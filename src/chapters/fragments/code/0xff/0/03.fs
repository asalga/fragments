// pendulum
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
#define E 0.01

float rSDF(vec2 p, vec2 dims){
  vec2 absValue = abs(p/dims);
  return max(absValue.x, absValue.y);
}

float cSDF(vec2 p, float r){
  return length(p) - r;
}

void main(){
  vec2 ar = vec2(1.,u_res.y/u_res.x);
  vec2 p = ar * (gl_FragCoord.xy / u_res * 2. -1.);
  
  float theta = sin(u_time * 4.) * PI/8.;
  mat2 rot = mat2(cos(theta), sin(theta), 
                  sin(theta), -cos(theta));
  
  vec2 bPos = vec2( sin(-theta) * 2., cos(1.5 * theta) - .5);
  float b = smoothstep(1.-E, 1., 1.-cSDF(p + bPos, 0.33)) - 
            smoothstep(1.-E, 1., 1.-cSDF(p + bPos, 0.3));

  float bh = smoothstep(1.-E, 1., 1.-cSDF(p + bPos, 0.25)) -
  			 smoothstep(1.-E, 1., 1.-cSDF(p + bPos + 
  			 vec2( cos(-theta) * 0.03, 
  			 	   sin(theta) * 0.03), 0.25));

  p.y -= 1.625;p *= rot;
  float a = 1.-smoothstep(1.-E, 1., rSDF(p, vec2(0.023, 1.8)));
  gl_FragColor = vec4(vec3(a+b+bh), 1.);
}