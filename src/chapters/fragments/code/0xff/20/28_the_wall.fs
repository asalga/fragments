precision mediump float;
uniform vec2 u_res;
uniform float u_time;
uniform vec3 u_mouse;
uniform sampler2D u_texture0;
uniform sampler2D u_texture1;

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);

  float t = u_time*2.;
  vec2 m = vec2(u_mouse);//(u_mouse.xy/u_res*2.-1.);;

  // p.x += t;

  vec2 uv = mod(p, 1.);
  vec3 dirLight = normalize(vec3(m.x, m.y, 1.));



  float ambientCol = 1.0;

  // get the color for the fragment
  vec4 diffuse = texture2D(u_texture0, uv);
  vec4 normal = texture2D(u_texture1, uv);

  vec3 n = normalize(vec3(vec2(normal),1.0));
  float theta = dot(n,dirLight);

  vec3 finalCol = vec3(diffuse) * theta;


  
  gl_FragColor = vec4(finalCol, 1.);
}