// Essentially just a 'pass-through' shader
precision mediump float;

uniform sampler2D lastBuffer;

void main(){
	vec2 p = gl_FragCoord.xy/vec2(300.);
	vec4 col = texture2D(p);
	gl_FragColor = vec4(col.rgb,1);
}