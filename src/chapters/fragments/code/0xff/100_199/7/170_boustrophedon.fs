precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const float Rows = 10.;
const float VertSpacing = .2;

float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res;
  float i;
  float t = u_time + 5.;

  t = mod(u_time, Rows) * .4;// * 0.;
  // p.y += u_time/15.;

  float rowID = floor(p.y * Rows);

  float y = floor(p.y * Rows);

  float _x;
  float _y = step(y, t-1.);


  // make the snake thinner
  float localY = fract(p.y*10.);
  float horizStrip = step(localY, 1.-VertSpacing) * step(VertSpacing, localY);

  // if floored time is on the current x/horizontal line that we are on
  float rowSelected = floor(t);
  if( rowID == rowSelected){
    float ft = fract(t);
    float x = p.x;

    // if even, we go backwards, so flip x
    if(mod(rowID,2.) == 0.){
      x = 1.-x;
    }

    // 0 if ft < edge
    // 1 if ft > edge
    _x = step(x, ft) * horizStrip;
  }

  vec2 p2 = (gl_FragCoord.xy/u_res)*2.-1.;
  float straightLineSpace = step(sdRect(p2, vec2(0.8, 1.)), 0.);




  i = _y * horizStrip * straightLineSpace;
  i += _x * straightLineSpace;



  vec2 grid = mod(p, .1) * Rows;
  // gl_FragColor = vec4(vec3(i,grid),1.);
  gl_FragColor = vec4(vec3(i),1.);
}
