precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdSquare(vec2 p, float halfW){
  vec2 _p = abs(p);
  return max(_p.x,_p.y)-halfW;
}
float square(vec2 p, float halfW){
  return smoothstep(0.01, 0.001, sdSquare(p, halfW));
}
float sdCircle(vec2 p, float r){
  return length(p)-r;
}
float circle(vec2 p, float r){
  return smoothstep(0.01, 0.001, sdCircle(p,r));
}
mat2 r2d(float a){
  return mat2(cos(a),-sin(a),sin(a),cos(a));
}
void main(){
  const float cnt = 10.;
  float time = u_time*4.;
  float t = time * 0.1;

  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  mat2 rot = r2d(-time);
  float sz = 0.45;
  float rad = 0.06;
  float i;

  // hack so I don't have to deal with hiding
  // circle that appear on far left
  p *= .9;

  
    for(float it=0.;it<10.;++it){
      vec2 v, shift, nudge;
      float offset;
      float pct = 0.2;

      offset = ((it/(cnt-1.))*2.-1.)*2. + pct;
      float mv = mod(t, pct);
      
      v = vec2(offset * sz - mv, -sz);
      nudge = vec2(0., -.2);
       
      float dist = sdSquare(v*rot, sz);
      shift = vec2(0., clamp(dist*1.5, -.4, .15));
      i += circle(p + v + shift + nudge, rad);
    }
  
    // too sleepy to fix this right now.
    // bottom
    for(float it=0.;it<10.;++it){
      vec2 v, shift, nudge;
      float offset;
      float pct = 0.2;

      offset = ((it/(cnt-1.))*2.-1.)*2. + pct;
      float mv = mod(-t, pct);
      
      v = vec2(offset * sz - mv, sz);
      nudge = vec2(0., -.25);
       
      float dist = sdSquare(v*rot, sz);
      shift = vec2(0., clamp(dist*1.5, -.4, .15));
      i += circle(p + v - shift + nudge + vec2(0., sz), rad);
    }

  i += square(p * rot, sz);
  gl_FragColor = vec4(vec3(i),1.);
}