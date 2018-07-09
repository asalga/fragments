precision mediump float;

uniform sampler2D buff;
uniform vec2 u_res;

void main(){
	vec2 p = gl_FragCoord.xy/vec2(300.);
	vec4 col = texture2D(buff, p);
	gl_FragColor = vec4(col.rgb, 1);
}