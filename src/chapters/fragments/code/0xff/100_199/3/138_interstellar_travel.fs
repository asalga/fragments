precision mediump float;

uniform vec2 u_res;
uniform float u_time;


float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

float random(vec2 p){
  const float X_SCALE = 568394.;
  const float Y_SCALE = 184362.;
  float r = sin(p.x*X_SCALE + p.y*Y_SCALE) * 1914234.;
  return fract(r);
}


float rand (vec2 st) {
  return fract(sin(dot(st.xy,vec2(122.9898,78.233)))*43758.5453123);
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  float i;

  vec3 stars[50];

  for(int it = 0; it < 24; ++it ){
    //float x = rand(p*4.)*2.-1.;
    // float y = rand(p)*2.-1.;
    float x;// = 0.5 * float(it)*9394300.;
    float y;// = 0.5 * float(it)*1234530.;

    // y = rand(float(it))*vec2(1234.,19234.)*10.;//*2.-1.;
    float _fit = float(it) + 1.0;
    vec2 r = vec2(_fit) * vec2(sin(121193.), sin(1987373.));

    float z = abs(rand(r))*4.;

    x = rand(r/100.) + ( 1./z * u_time/2.);

    float f = floor(x);

    y = rand(r/80.);

    // y = rand(vec2(f,f)) * y;

    stars[it].xy = fract(vec2(x,y))*2.-1.;

    i += step(sdRect(stars[it].xy + p, vec2(z/100.)), 0.);


    // for(int it = 0; it < 5; ++it){
    //   float _fit = float(it);
    //   vec2 pos = stars[it].xy + p;
    //   pos.x -= _fit/20.;

    //   float factor = (1./ (_fit*2. + 1.0));

    //   i += step(sdRect( pos , vec2(z/100.)* factor), 0.) * factor;
    // }

  }

  gl_FragColor = vec4(vec3(i),1);
}