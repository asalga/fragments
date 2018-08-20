precision mediump float;

uniform sampler2D u_t0;
uniform float u_time;
uniform vec2 u_res;

void main(){
  float t = u_time;
  vec2 p = (gl_FragCoord.xy/u_res);
  p.y = 1.-p.y;

  float marker = 0.5;
 // if(marker < p.x){
   //p.x +=  marker;
  //}


  vec3 c = texture2D(u_t0, p).rgb;

   c += texture2D(u_t0, p + vec2(marker*2.,0.) ).rgb;

  gl_FragColor = vec4(c, 1);
}