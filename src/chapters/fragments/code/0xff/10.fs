precision mediump float;
uniform vec2 u_res;
uniform float u_time;

#define COS_30 0.8660256249

float rectSDF(vec2 p, vec2 size){
  vec2 absValue = abs(p / size);
  float side = max(absValue.x, absValue.y);
  return smoothstep(side-0.1,side, size.x);// * step(side, size.y);
}

// Function from Iñigo Quiles
vec3 hsb2rgb( in vec3 c ){
    vec3 rgb = clamp(abs(mod(c.x*6.0+vec3(0.0,4.0,2.0),
                             6.0)-3.0)-1.0,
                     0.0,
                     1.0 );
    rgb = rgb*rgb*(3.0-2.0*rgb);
    return c.z * mix(vec3(1.0), rgb, c.y);
}

mat2 r2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}

float hlSDF(vec2 p, float w){  
  vec2 a = abs(p);
  return length(vec2(max(a.x-w,0.), a.y));
}

vec3 spectrum(vec2 p){
	p *= r2d(0.1);
  vec2 a = (p+1.)/2.;
  a.y += u_time*.1;
  a.y *= 8.;
  return hsb2rgb(vec3( a.y , 1., 1.));
}

float tSDF(vec2 p, float s){
  vec2 a = abs(p);
  // Take the horizontal distance to the boundry and
  // scale by cos 30 giving us a diagonal line to the object.
  float distToSide = a.x * COS_30;
  float u = p.y * 0.5;
  // we are offsetting the triangle
  float _1 = distToSide + u;
  // max - for the bottom part. we're preventing 
  // any fragments from satisfying the case of the 
  // triangle being taller than desired.
  return max(_1,-u) - s;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);  
  vec2 p = a * vec2(gl_FragCoord.xy/u_res * 2. -1.);
  
  vec3 i = vec3(1.-step(0.,tSDF(p, .25)));

  float t = u_time*1.;
  vec2 rr = vec2(cos(t), -sin(t));

  vec3 r = 
  vec3(rectSDF(p* r2d(0.1) + vec2(-1., 0.), 
  	vec2(1., .08))) * spectrum(p);
  i += r;

  i *= vec3(step(0.,tSDF(p, .24)));
  

  i += vec3(step(p.x, 0.0) * step(abs(p.y),0.005));
  
  gl_FragColor = vec4(vec3(i), 1.);
}
