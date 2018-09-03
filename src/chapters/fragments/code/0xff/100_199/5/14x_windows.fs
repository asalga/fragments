precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

void main(){
  float i;
  float numSlices = 5.0;
  vec2 np = gl_FragCoord.xy/u_res;
  float t = u_time/1.;

  // rect coords
  vec2 rc = vec2(fract(np*numSlices) - 0.5);
  vec2 id = floor(np*numSlices);

  // if(id > 2. && id < 10.){
    i += step(sdRect(rc - vec2(2.0, 0.) + vec2(t,0.), vec2(0.45)), 0.);
    i += step(sdRect(rc - vec2(1.0, 0.) + vec2(t,0.), vec2(0.45)), 0.);

    i += step(sdRect(rc + vec2(0., 0.) + vec2(t,0.), vec2(0.45)), 0.);


  // }

  vec3 i3 = vec3(i); + vec3(fract(np.x*numSlices));

  gl_FragColor = vec4(vec3(i3),1);
}