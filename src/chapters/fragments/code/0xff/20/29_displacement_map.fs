precision mediump float;
uniform vec2 u_res;
uniform float u_time;
uniform sampler2D u_texture0;
uniform sampler2D u_texture1;

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  
  float t = u_time*1.;
  float dispScale = .01;

  vec2 diffUV = mod(p/2.,1.);
  vec2 dispUV = mod(p/vec2(2., 3.)+vec2(0.5, 0.5) + vec2(t/2., 0.0),1.);

  // each pixel is displaced 'k' pixels up and left
  float dispIntensity = texture2D(u_texture1, dispUV).r;

  // dispIntensity *= (1.+sin(t)/2.);

  float disp = dispScale * dispIntensity;
  // disp = vec2(0.);

  vec4 col = texture2D(u_texture0, mod(diffUV + disp, 1.));
  // vec4 col = texture2D(u_texture1, mod(dispUV + disp, 1.));

  gl_FragColor = vec4(col.xyz, 1.);
}
