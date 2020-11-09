section .data

lhand:  dw 1 ; reserve word for left-hand
rhand:  dw 1 ; reserve word for right-hand


section .text

global _start

_start:

    mov eax, lhand
    mov ebx, rhand
    add eax, ebx    ; result stored in ecx

    ret
