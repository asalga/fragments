precision mediump float;
uniform vec2 u_res;

float grid(){
 vec2 p = gl_FragCoord.xy;
 vec2 lineSzPx = vec2(1.);
 vec2 cellSzPx = vec2(50.);
 vec2 i = step(mod(p,cellSzPx),lineSzPx);
 return i.x + i.y;
}

void main(){
 vec2 a = vec2(1.,u_res.y/u_res.x);
 vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
 float i = grid();
 gl_FragColor = vec4(vec3(i),1.);
}