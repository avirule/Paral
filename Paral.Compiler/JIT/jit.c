#include <Windows.h>
#include <stdint.h>

#define PAGE_SIZE 4096

#define EAX 0x66
#define EDX 0xD0

#define MOV 0x89
#define ADD 0x1
#define SUB 0x29

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

void main() {
    struct asmbuffer_insert* asmbuffer = asmbuffer_create();
    asmbuffer_insert(asmbuffer, 3, 0x4889C8);
    asmbuffer_insert(asmbuffer, 1, 0xC3);

    asmbuffer_insert(asmbuffer, 2, 0x48B8);
    long operand = 0xFFFF;
    asmbuffer_immediate(asmbuffer, 8, &operand);

}