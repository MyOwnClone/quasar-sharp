

mov	%r0,	0xF0
mov	%r1,	100
mov	@r0,	%r1
mov	%r1,	117
mov	@r0:0x04 ,	%r1

mov	%r3,	@r0
mov	%r4,	@r0:0x04
loop:	
br	loop

