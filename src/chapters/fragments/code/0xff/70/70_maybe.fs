// 70 - "Maybe"
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdCircle(vec2 p, float r){
	return length(p)-r;
}

float circle(vec2 p, float r){
  return step(sdCircle(p, r), 0.);
}

//from bookofshaders
float capsule(vec2 p, vec2 a, vec2 b, float r) {
    vec2 pa = p - a, ba = b - a;
    float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
    return length( pa - ba*h ) - r;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time*1.025;
  float fractTime = fract(t);
  float i;
  
  float startX = -.25;
  vec2 trans = vec2(startX + fractTime*.5, 0.);

  i = circle(p+trans, 0.25);
 
  float inv = step(mod(t,2.), 1.);// 0 | 1 
  i = abs(inv - i);

  gl_FragColor = vec4(vec3(i),1.);
}