#include <Windows.h>
#include <stdint.h>
#include <string.h>
#include <stdio.h>

#define PAGE_SIZE 4096

#define TEST_ASM "mov eax, edx\0"

struct asmbuffer
{
    uint8_t code[PAGE_SIZE - sizeof(uint64_t)];
    uint64_t count;
};

struct asmbuffer *asmbuffer_create()
{
    DWORD type = MEM_RESERVE | MEM_COMMIT;
    return VirtualAlloc(NULL, PAGE_SIZE, type, PAGE_READWRITE);
}

void asmbuffer_finalize(struct asmbuffer *buffer)
{
    VirtualFree(buffer, 0, MEM_RELEASE);
}

void admbuffer_finalize(struct asmbuffer *buffer)
{
    DWORD old;
    VirtualProtect(buffer, sizeof(*buffer), PAGE_EXECUTE_READ, &old);
}

void asmbuffer_insert(struct asmbuffer *buffer, int size, uint64_t insert) {
    buffer->code[buffer->count] = insert;
    buffer->count += size;
}
void asmbuffer_immediate(struct asmbuffer *buffer, int size, const void *value) {
    buffer->code[buffer->count] = (uint8_t*)value;
    buffer->count += size;
}

typedef uint32_t (WINAPI *FASMASSEMBLE)(char * szSource, unsigned char * lpMemory, int nSize, int nPassesLimit);

#pragma pack(push, 1)
typedef struct {
    uint32_t condition;
    uint32_t output_length;
    uint32_t* output_data;
} *FASM_STATE;
#pragma pack(pop)

int main() {
    // struct asmbuffer_insert* asmbuffer = asmbuffer_create();
    // asmbuffer_insert(asmbuffer, 3, 0x4889C8);
    // asmbuffer_insert(asmbuffer, 1, 0xC3);

    // asmbuffer_insert(asmbuffer, 2, 0x48B8);
    // long operand = 0xFFFF;
    // asmbuffer_immediate(asmbuffer, 8, &operand);

    HMODULE libHandle;

    if ((libHandle = LoadLibrary(TEXT("..\\Reference\\FASMX64.DLL"))) == NULL) {
        printf("load failed\n");
        return -1;
    }

    FASMASSEMBLE fasmAssemble;
    if ((fasmAssemble = (FASMASSEMBLE)GetProcAddress(libHandle, "fasm_Assemble")) == NULL) {
        printf("failed to get address of function\n");
        return GetLastError();
    }

    uint8_t* memory = malloc(PAGE_SIZE);
    fasmAssemble(&TEST_ASM, memory, PAGE_SIZE, 100, NULL);

    FASM_STATE fasm_state = (FASM_STATE)memory;

    printf("%d\n", fasm_state->condition);
    printf("%d\n", fasm_state->output_length);

    for (uint32_t index = 0; index < PAGE_SIZE; index++) {
        printf("%d ", memory[index]);
    }

    return 1;
}