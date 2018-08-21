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
  const int NumStars = 35;

  vec3 stars[NumStars];

  for(int it = 0; it < NumStars; ++it ){
    // y = rand(float(it))*vec2(1234.,19234.)*10.;//*2.-1.;
    float _fit = float(it) + 1.0;
    vec2 r = vec2(_fit) * vec2(sin(121193.), sin(1987373.));

    float z = abs(rand(r))*4.;

    float x = rand(r/100.) + (1./z * u_time/2.);

    float f = floor(x) + 1.0;
    // y = rand(r/80.);
    float y = rand(vec2(f,f)) * z;
    y = y * 2.0 -1.;

    stars[it].xy = fract(vec2(x,y))*2.-1.;

    i += step(sdRect(stars[it].xy + p, vec2(1./z)*0.01 ), 0.);
  }

  gl_FragColor = vec4(vec3(i),1);
}