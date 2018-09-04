precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdCircle(vec2 p, float r){
  float l = length(p)-r;
  if(l < 0.) l = .0;

  return l;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = u_time*2.;
  p *= 1.25;

  for(int it = 1; it <= 30; ++it){
    float fit = 1./float(it);
    t += fit*2.;
    float x = cos(t);
    float y = sin(t*2.)/2.;
    vec2 mov = vec2(x,y);
    i += (.01/sdCircle(p+mov,0.0025 * (1./fit) ))/2.;
  }


  float flip = sin(t);

  // i *= step(p.x, p.y);

  // if(step(p.y,p.x) >= 1.){

  if(flip > 0.){
    if(p.y > 0.){
      // i = 1.-i;
    }
  }
  else{
   if(p.y < 0.){
      // i = 1.-i;
    }
  }



  gl_FragColor = vec4(vec3(i),1.);
}