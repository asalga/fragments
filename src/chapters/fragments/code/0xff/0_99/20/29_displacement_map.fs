precision mediump float;
uniform vec2 u_res;
uniform float u_time;
uniform vec2 u_tracking;
uniform sampler2D u_texture0;
uniform sampler2D u_texture1;
uniform sampler2D u_texture2;

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  vec2 m = u_tracking * vec2(1., -1.)/3.;
  
  float t = u_time*.5;
  float dispScale = .02;

  vec2 diffUV = mod(p/3.,1.);
  vec2 dispUV = mod(p/3.+m*t,1.);
  vec2 causUV = mod((p/2.)+t/13.,1.);

  float dispI = texture2D(u_texture1, dispUV).r;
  float disp = dispScale * dispI;

  vec3 causticsCol = texture2D(u_texture2, mod(causUV+disp*3., 1.)).xyz;
  vec3 col = texture2D(u_texture0, mod(diffUV + disp, 1.)).xyz * causticsCol;

  gl_FragColor = vec4(vec3(col.rgb), 1.);
}
