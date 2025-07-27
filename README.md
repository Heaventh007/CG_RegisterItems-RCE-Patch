# CG_RegisterItems-RCE-Patch
This is an Xbox 360 tool to patch a buffer overflow in **CG_RegisterItems**.

Thankfully, without a bypass, this exploit **DOES NOT WORK** on retail consoles. \
All it can do is trigger a stack protection and freeze the console.

## The Exploit:

**CG_RegisterItems** takes in a string returned by **CL_GetConfigString** and copies it into a stack allocated buffer. \
**CL_GetConfigString** can return a value ***much larger*** than the size of the stack allocated buffer.

<img width="806" height="492" alt="image" src="https://github.com/user-attachments/assets/8f57cabb-837a-4f31-8114-b0d29d44fef6" />


## Patching this exploit:

In this repository, I've included a patch in a tool. \
Simply connect to your console, and click **Patch Exploit**!

### Here is a look at how I've patched this exploit:
```asm
; Do the original instructions we overwrote... 
stw r10, 0x150(r1) ; Store the stack cookie onto the stack

; Call CL_GetConfigString
lis r0, 0x8226
ori r0, r0, 0x28D8
mtctr r0
bctrl

; This shouldn't happen, but let's check anyway.
cmpwi cr6, r3, 0 
beq cr6, jBadCall

; Call I_DrawStrlen
mr r31, r3
lis r0, 0x8249
ori r0, r0, 0xE158
mtctr r0
bctrl

; Check the length of the input buffer.
cmplwi cr6, r3, 256 ; Our buffer size is 256
bge cr6, jBadCall ; Branch away if the length of the input buffer is too large.

; Get us ready to jump back to the game
mr r3, r31

addi r9, r1, 0x50
subf r11, r3, r9

; Jump after our hook.
lis r0, 0x8224
ori r0, r0, 0xB19C
mtctr r0
bctr

jBadCall:
; Jump to end of function.
lis r0, 0x8224
ori r0, r0, 0xB240
mtctr r0
bctr
```

We overwrite some instructions right before the call to **CL_GetConfigString** to jump to our custom payload:
<img width="1085" height="357" alt="image" src="https://github.com/user-attachments/assets/796ce84c-e128-478b-b211-51d3e685d618" />

Write a patch to jump to our main patch.
```asm
lis r0, 0x8294
ori r0, r0, 0x5348
mtctr r0
bctr ; Jump to our custom instructions
```
