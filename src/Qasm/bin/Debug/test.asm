main:


	mov	%r0,	msg
	mov	%r1,	0xB8000

.loop:
	mov	%r2,	@r0
	and	%r2,	0xFF
	bz	.done
	mov	@r1,	%r2
	add	%r1,	1
	add	%r0,	1
	mov	%r2,	0x0F
	mov	@r1,	%r2
	add	%r1,	1
	br	.loop
.done:
	wait
	br	.done
	rtn

msg:
	string	"Hello, World!"
