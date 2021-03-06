// 126 - "Pixel City" adapted from #53
precision mediump float;

uniform float u_time;
uniform vec2 u_res;

const float PI = 3.141592658;

float cSDF(vec2 p, float r){
  return length(p) - r;
}

void main(){
  vec2 a = vec2(1.,u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time*.0;

  float theta = abs(atan(p.y,p.x))/PI;
  
  float i = step(cSDF(p,.45), 0.) * 
  //smoothstep(0.01,0.001,cSDF(p,.45)) * 
        step(.25,theta+(sin(t*PI*2.)+1.)/2.*.25);
  i += step(cSDF(p+vec2(mod(t,1.)-1.,0.),.08),0.);

  i = cSDF(p, .5);

  gl_FragColor = vec4(vec3(i),1.);
}