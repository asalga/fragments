precision mediump float;
uniform vec2 u_res;
#define PI 3.141592658

float f(float v){
  return sin(v);
}

float rectSDF(vec2 p, vec2 size) {  
  vec2 d = abs(p) - size;
  return min(max(d.x, d.y), 0.0) + length(max(d,0.0));
}

float expStep( float x, float k, float n )
{
    return exp( -k*pow(x,n) );
}
float impulse( float k, float x )
{
    float h = k*x;
    return h*exp(1.0-h);
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float x = ((p.x+1.)/2.)*PI;
  float y = p.y+1.;
  float v;
  
  // float v = f(x);
  // float v = impulse(10., x);
  v = expStep(x,2.3, 1.);

  float i =  step(y,v*2.);
  // i+= step(rectSDF(p+vec2(1.,0.0), vec2(0.3, 1.)), 0.);

  gl_FragColor = vec4(vec3(i),1.);
}