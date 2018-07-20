precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define SZ 0.25
float squareSDF(vec2 p){
  return max(abs(p).x, abs(p).y);
}
vec3 tile(vec2 p, float debug){
  vec2 p0 = vec2(p.x-SZ, p.y);
  p0.y -= p0.x;//skew
  vec2 p1 = vec2(p.x+SZ, p.y);
  p1.y += p1.x;//skew

  float fq = 0.07;
  vec3 darkShade = vec3(1.) * step(mod(p.x, fq), fq/2.);
  vec3 lightShade = vec3(1.) * step(mod(p.y, fq), fq/2.);

  vec3 c = darkShade * step(squareSDF(p0), .25) + 
  		     lightShade * step(squareSDF(p1), .25);
  c.r += debug;
  return c;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  
  // move set of blocks down based on time
  vec3 droppedRow;
  p *= 2.5;
  p.y += mod(u_time, 1.);

  float test = 0.;

  // if(p.y > 0. ){
  	// test = 1.;
  // }

  vec2 pt = vec2(0.);
  // float X = step(mod(p.y,2.),1.)*.5;

  if(p.y < 0.){
    pt.x = mod(p.x + 0., 1.);
    pt.y = mod(p.y, 1.);
  }

  vec3 c = tile(pt+vec2(-.5), 0.);

  vec2 dt = pt;

  vec2 fin = vec2(0., -1.4);
  vec2 v = fin * (1.-mod(u_time, 1.));
  droppedRow = vec3(1.) * 1.-step(0.25,squareSDF(p+v));

  tile(dt+vec2(-.5), .3) * step(3., p.y);

  // dt.y *= u_time*2.;// * mod(u_time, 1.7);
  // droppedRow = tile(dt+vec2(-.5), .3) * 
  				// step(3., p.y);

  // for(int i= 0; i < 2; i++){
  // 	float Y = pt.y + 3.;
  // 	float X = pt.x;
  // 	vec2 _ = p - vec2(X, Y );
  // 	droppedRow = tile(_, 0.3);
  // 		//dt+vec2(-.5), .3); 
  // }


  if(test == 1.){
  	// c = vec3(0.);
  }

  // c += droppedRow;

  gl_FragColor = vec4(vec3(c), 1.);
}