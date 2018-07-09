precision mediump float;

uniform sampler2D buff;
uniform vec2 u_res;

void main(){
	vec2 p = gl_FragCoord.xy/vec2(300.);
	vec4 col = texture2D(buff, vec2(0.5));
	gl_FragColor = vec4(0, col.g, 0, 1);
}