precision mediump float;
uniform vec2 u_res;
uniform float u_time;
uniform vec3 u_mouse;
uniform sampler2D u_texture0;
uniform sampler2D u_texture1;

void main(){  
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time*.5;
  // p.x += t;
  float zScale = .5;
  vec3 m = vec3((u_mouse.xy/u_res*2.)-1., 1.);
  
  vec3 dirLight = normalize(m);
  // vec3 pointLight = 


  vec2 uv = mod(p, 1.);
  vec4 diffuse = texture2D(u_texture0,uv);
  vec4 normal = texture2D(u_texture1,uv);

  vec3 n = vec3(normal.xy*2.-1.,normal.z*zScale);
  n = normalize(n);

  float theta = dot(n, dirLight);

  vec3 finalCol = vec3(diffuse) * theta;
  finalCol = vec3(theta);
  
  gl_FragColor = vec4(finalCol, 1.);
}