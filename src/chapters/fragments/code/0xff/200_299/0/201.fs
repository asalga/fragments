// 75 Shatter/Plasma
precision mediump float;

uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = u_time*1.;

  // i += sin(p.x * 10. + t);// * sin(u_time+2.);
  // i += sin(length(p*10.)) * sin(t*0.25);

  float r = sin(p.x * 10.0 + sin(p.y*10.0));
  float g = cos(p.y * 1.0 + t);
  float b = sin(t * 2.0);
  float r2 = cos(p.x * 10.0 + sin(p.y*1.0));

    // vec3 col = vec3(r*g, r+g,
  i = r*g + r+g;// + r2;

  // if(i >0.){i = 1.;}
  i = smoothstep(i, 0.1, 0.01);

  // i += sin(t+length(p*10.)*PI) * cos(-u_time);
  // i += sin((p.x+p.y) * 5. +t);// * sin(u_time);

  gl_FragColor = vec4(vec3(i),1.);
}