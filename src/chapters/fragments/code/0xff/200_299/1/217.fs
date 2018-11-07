precision mediump float;

uniform vec2 u_res;
uniform float u_time;
const float sz = .25;

float sdCircle(in vec2 p, float r){
  return length(p) -r;
}

float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}
// from iq
float sdEqTri( in vec2 p){
    float k = sqrt(3.0);
    p.x = abs(p.x) - 1.0;
    p.y = p.y + 1.0/k;
    if( p.x + k*p.y > 0.0 ) p = vec2( p.x - k*p.y, -k*p.x - p.y )/2.0;
    p.x -= clamp( p.x, -2.0, 0.0 );
    return -length(p)*sign(p.y);
}
// from iq
float sdTriangle( in vec2 p, in vec2 p0, in vec2 p1, in vec2 p2 ){
    vec2 e0 = p1-p0, e1 = p2-p1, e2 = p0-p2;
    vec2 v0 = p -p0, v1 = p -p1, v2 = p -p2;

    vec2 pq0 = v0 - e0*clamp( dot(v0,e0)/dot(e0,e0), 0.0, 1.0 );
    vec2 pq1 = v1 - e1*clamp( dot(v1,e1)/dot(e1,e1), 0.0, 1.0 );
    vec2 pq2 = v2 - e2*clamp( dot(v2,e2)/dot(e2,e2), 0.0, 1.0 );

    float s = sign( e0.x*e2.y - e0.y*e2.x );
    vec2 d = min(min(vec2(dot(pq0,pq0), s*(v0.x*e0.y-v0.y*e0.x)),
                     vec2(dot(pq1,pq1), s*(v1.x*e1.y-v1.y*e1.x))),
                     vec2(dot(pq2,pq2), s*(v2.x*e2.y-v2.y*e2.x)));

    return -sqrt(d.x)*sign(d.y);
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res;
  float i = 0.;
  float sz = 1.;

  float halfSz = sz/2.;
  float h = sqrt(sz*sz - halfSz*halfSz );
  float REP = 4.;
  float t = u_time * 1.;

  // p.y -= t*2.;


  // if( floor(p.x * REP) == 0.){
    // p.y -= t*2.;
  // }

   // p.y -= t*2. * floor(p.x);

   // p.y -= fract(t * floor(p.x * REP));

   if(fract(t) < .25 && floor(p.x * REP) == 0.){
    p.y += t * h;
   }
   if(fract(t) < .5 && floor(p.x * REP) == 1.){
    p.y += t * h;
   }
   if(fract(t) < .75 && floor(p.x * REP) == 2.){
    p.y += t * h;
   }
    if(fract(t) < 1. && floor(p.x * REP) == 3.){
    p.y += t * h;
   }

  vec2 co = vec2(sz, h) * (1./REP);
  vec2 rp = mod(p, co);

  vec2 id = floor(p * REP);

  vec2 a = vec2(0, 0);
  vec2 b = vec2(co.x, 0);
  vec2 c = vec2(co.x/2., co.y);


  if( mod(id.x, 2.) == 0.){
    i = step(sdTriangle(rp , a, b, c), 0.) * 0.25;
    // i *= step(sin(length(rp)) *  sdCircle(rp, sz), 0.);
    // i *= step(sdCircle(rp, 0.25), 0.);
  }
  else {


    t = 0.;
    // float f1 = mod(t*2. * fract(id.x/10.), h*0.25);
    // float f2 = mod(t*2. * fract(id.x/10.), h*0.25);
    float f1 = mod(-t*4., h*0.25);
    float f2 = mod(-t*4., h*0.25);

    a.y = -a.y;
    b.y = -b.y;
    c.y = -c.y;

    i += step(sdTriangle(rp - vec2(0., f1       ), a, b, c), 0.)*0.25;// * .75;
    i += step(sdTriangle(rp - vec2(.0, f2 + h/4.), a, b, c), 0.)*0.25;// * .75;

  }

  gl_FragColor = vec4(vec3(i),1.);
}