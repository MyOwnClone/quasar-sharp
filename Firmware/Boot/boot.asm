org	0xFFD00000

power_on:

	mov	%r0,	0xFF9F0000
.nextimg:

	mov	%r1,	@r0
	add	%r0,	1
	mov	%r2,	@r0
	add	%r0,	1
	mov	%r3,	@r0
	add	%r0,	1
	cmp	%r3,	0
	bz	.done
	mov	%r11,	.nextimg
	mov	%r15,	%r2
.done:
	mov	%r15,	0x00000000

