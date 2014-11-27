puts:

.loop:

	mov	%r2,	@r0
	and	%r2,	0xFF
	bz	.end
	add	%r0,	1

	mov	%r2,	0x01
	mov	%r3,	0xFFE00005
	mov	@r3,	%r2
	mov	%r2,	0
	add	%r3,	1
	mov	@r3,	%r2
	mov	%r2,	0x1
	mov	@r3,	%r2
	br	.loop

	.end:

        rtn

