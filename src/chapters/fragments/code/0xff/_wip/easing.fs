
float q (float t, float b, float c, float d) {
  t /= d;
  return c*t*t*t*t*t + b;
}


float inOutQuart (float t, float b, float c, float d) {
  t /= d/2.;
  if (t < 1.) return c/2.*t*t*t*t + b;
  t -= 2.;
  return -c/2. * (t*t*t*t - 2.) + b;
}

float e(float t,float b,float c,float d) {
  t /= d/2.;
  if (t < 1.) return c/2.*t*t + b;
  t--;
  return -c/2. * (t*(t-2.) - 1.) + b;
}

float OutQuart (float t, float b, float c, float d) {
  t /= d;
  t--;
  return -c * (t*t*t*t - 1.) + b;
}

