/*
    Andor Saga
*/
'use strict';

let relPath = '../../chapters/fragments/code/0xff/';

let demo = {
  'size': {
    'width': 500,
    'height': 500
  },
  '0': {
    // src: '100_199/2/124_dither_test.fs',
    // src: '100_199/2/127.fs',
    // src: '100_199/2/128_half_tone.fs',
    // src: '100_199/2/129_v.fs',
    // src: '100_199/2/129.fs',
    src: '100_199/3/132.fs',
    
    uniforms: [
      // {'name': 'u_fov', 'value': 70}
    ]
  },
  '1': {
    // src: 'post_process/simple_dither.fs',
    src: 'post_process/null.fs',
    // src: 'post_process/cel.fs',
    // src: 'post_process/pixelate.fs',
    // src: 'post_process/sobel.fs',

    uniforms: [
      { 'name': 'u_numShades', 'value': 12 },
      {
        'name': 'u_pixelSize',
        'value': function(s) {
          return 8.;
          return Math.sin(s / 10.) * 20;
        }
      },
      { 'name': '_', 'value': [-1, -1, 0, -1, 1, -1, -1, 0, 0, 0, 1, 0, -1, 1, 0, 1, 1, 1] }
    ]
  }
};


let globalVs = `
precision highp float;
attribute vec3 aPosition;

uniform mat4 uModelViewMatrix;
uniform mat4 uProjectionMatrix;

void main() {
  gl_Position = uProjectionMatrix * uModelViewMatrix * vec4(aPosition, 1.0 );
}`;


let gif, YesMakeGif = false;

Number.prototype.clamp = function(min, max) {
  return Math.min(Math.max(this, min), max);
};

function animate(time) {
  requestAnimationFrame(animate);
  // TWEEN.update(time);
}
requestAnimationFrame(animate);


/*
  fs - array of frag shaders
*/
function makeSketch(fs, params) {
  const DefaultSketchWidth = 320;
  const DefaultSketchHeight = 240;

  let w, h;
  let img0, img1, img2;

  // let timeVal = { t: 0 };
  let sketchTime;
  let tracking = [];
  let easing = 0.05;
  let start = 0;
  let mouseIsDown = 0;
  let lastMouseDown = [120, 200];
  // let shader_0_Frag, shader_1_Frag;

  let gfx, shader_0, shader_1;
  let shader_0_Frag = fs[0];
  let shader_1_Frag = fs[1];
  let ditherTex;
  let ditherMat = [
    7, 9, 5,
    2, 1, 4,
    6, 3, 8
  ];

  var sketch = function(p, relPath) {



    p.preload = function() {
      let mainFs = `precision mediump float;
                    uniform sampler2D lastBuffer;
                    void main(){
                      vec2 p = gl_FragCoord.xy/vec2(300.);
                      vec4 col = texture2D(lastBuffer, vec2(0.5));
                      gl_FragColor = vec4(col.rg,1, 1);
                    }`;
    };

    p.setup = function() {
      w = params.width || DefaultSketchWidth;
      h = params.height || DefaultSketchHeight;
      sketchTime = 0;
      tracking = [0, 0];

      ditherTex = p.createGraphics(3, 3, p.P2D);
      ditherTex.pixelDensity(1);
      ditherTex.canvas.style = ''; // remove the 'display:none' to see the cvs

      // Dither texture/matrix
      // let ditherMat = [
      // 2,5,3,4,1,6,7,8
        // 7, 9, 5,
        // 2, 1, 4,
        // 6, 3, 8
      // ];

      for (let x = 0; x < 3; ++x) {
        for (let y = 0; y < 3; ++y) {
          let col = (ditherMat[y * 3 + x] / 10) * 255;
          ditherTex.stroke(col);
          ditherTex.point(x, y);
        }
      }

      p.createCanvas(w, h, p.WEBGL);
      gfx = p.createGraphics(w, h, p.WEBGL);
       gfx.pixelDensity(1);

      shader_0 = new p5.Shader(gfx._renderer, globalVs, shader_0_Frag);
      shader_1 = new p5.Shader(p._renderer, globalVs, shader_1_Frag);

      p.pixelDensity(1);
      $(p.canvas).appendTo($('#target'));
      $(ditherTex.canvas).appendTo($('#target2'));
      // p.noLoop();
    };

    p.mouseClicked = function() {
      start = p.millis();
    };

    /**
      Draw
    */
    p.draw = function() {
      let [width, height] = [w, h];
      let sz = 1.;
      sketchTime = (p.millis() - start) / 1000;

      // totally mess with the dithering :)
      // if( p.frameCount % 40 == 0){
      //   for(let i = 0; i < 30; i++){
      //     let _one = Math.floor(Math.random()*9);
      //     let _two = Math.floor(Math.random()*9);
      //     [ditherMat[_one], ditherMat[_two]] = [ditherMat[_two], ditherMat[_one]] ;
      //   }
      // }

      // // Create our dither texture
      // for (let x = 0; x < 3; ++x) {
      //   for (let y = 0; y < 3; ++y) {
      //     let col = (ditherMat[y * 3 + x] / 10) * 255;
      //     ditherTex.stroke(col);
      //     ditherTex.point(x, y);
      //   }
      // }

      gfx.push();
      gfx.translate(width / 2, height / 2);
      gfx.shader(shader_0);

      shader_0.setUniform('u_res', [width, height]);
      shader_0.setUniform('u_mouse', [p.mouseX.clamp(0, w) / width, p.mouseY.clamp(0, h) / height, mouseIsDown]);
      shader_0.setUniform('u_time', sketchTime);
      demo[0].uniforms.forEach(v => { // custom uniforms
        shader_0.setUniform(v.name, v.value);
      });

      gfx.rect(-width * sz, -height * sz, width * sz, height * sz, 2, 2);
      gfx.pop();




      // Post Processing
      p.push();
      p.translate(width / 2, height / 2);
      p.shader(shader_1);
      shader_1.setUniform('u_res', [width, height]);
      shader_1.setUniform('u_time', sketchTime);
      shader_1.setUniform('u_t0', gfx);
      // shader_1.setUniform('u_pixelSize', 1.+Math.floor(p.mouseX/10) );
      shader_1.setUniform('u_ditherTex', ditherTex);

      demo[1].uniforms.forEach(v => { // custom uniforms
        let val = typeof v.value === 'function' ? v.value(sketchTime) : v.value;
        shader_1.setUniform(v.name, val);
      });

      p.rect(-width * sz, -height * sz, width * sz, height * sz, 2, 2);
      p.pop();

    }; //end draw
  };
  return sketch;
}



function getFs0() {
  return fetch(relPath + demo[0].src)
    .then(res => res.text())
}

function getFs1() {
  return fetch(relPath + demo[1].src)
    .then(res => res.text())
}


(function load() {
  Promise.all([getFs0(), getFs1()])
    .then(fragShaders => {
      
      let sketch = new p5(makeSketch(fragShaders, demo.size), relPath);
    });
})();