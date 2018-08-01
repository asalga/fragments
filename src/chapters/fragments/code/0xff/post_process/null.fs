/*
	Right now, every sketch uses a post-processing shader
	just to make things easy.

	If none is needed, just use this, which acts
	as a pass-through.
*/

precision mediump float;

uniform sampler2D u_t0;
uniform vec2 u_res;

void main() {
  vec2 p = gl_FragCoord.xy / u_res;
  p.y = 1.0 - p.y;
  gl_FragColor = texture2D(u_t0, p);
}
