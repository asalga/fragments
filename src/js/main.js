/*
    Andor Saga
*/

'use strict';

Number.prototype.clamp = function(min, max) {
  return Math.min(Math.max(this, min), max);
};

function animate(time) {
  requestAnimationFrame(animate);
  TWEEN.update(time);
}
requestAnimationFrame(animate);

function makeSketch(fs, params) {
  const DefaultSketchWidth = 320;
  const DefaultSketchHeight = 240;
  let sh, w, h;
  let img0;

  let timeVal = { t: 0 };
  let sketchTime;
  let tracking = [];
  let easing = 0.05;
  let start =0;
  let mouseIsDown = 0;
  let lastMouseDown= [120,200];

  var sketch = function(p) {

    p.preload = function() {
      let vs = `precision highp float;
                varying vec2 vPos;
                attribute vec3 aPosition;
                void main() {
                  vPos = (gl_Position = vec4(aPosition,1.0)).xy;
                }`;
      sh = p.createShader(vs, fs);

      if (params.tex0) {
        img0 = p.loadImage(params.tex0);
      }
    };

    p.setup = function() {
      w = params.width || DefaultSketchWidth;
      h = params.height || DefaultSketchHeight;
      sketchTime = 0;
      tracking = [0,0];

      var c = p.createCanvas(w, h, p.WEBGL);
      p.pixelDensity(1);

      // reset anim
      c.mouseClicked(e=>{
        start = p.millis();
      });
      c.mousePressed(e=>{
        mouseIsDown = 1;
      });
      c.mouseReleased(e=>{
        mouseIsDown = 0;

        let x = p.mouseX.clamp(0, w);
        let y = p.mouseY.clamp(0, h);
        lastMouseDown = [x,y];
      });

      // c.mouseOver(e => {
      //   TWEEN.removeAll();
      //   new TWEEN.Tween(timeVal)
      //     .to({ t: 1 }, 2500)
      //     .easing(TWEEN.Easing.Quadratic.Out)
      //     .start();
      //   p.loop();
      // });

      // c.mouseOut(e => {
      //   TWEEN.removeAll();
      //   new TWEEN.Tween(timeVal)
      //     .to({ t: 0 }, 2000)
      //     .easing(TWEEN.Easing.Quadratic.In)
      //     .onComplete(() => p.noLoop())
      //     .start();
      // });

      p.loop();
    };

    p.draw = function() {
      // for tweening in animation
      sketchTime += (1 / 60) * timeVal.t;
      //sketchTime = p.millis() / 1000 * 0.5;

      // for resetting animation
      sketchTime = (p.millis()-start) / 1000 * 0.5;

      p.shader(sh);

      if (fs.match(/uniform\s+vec2\s+u_res/)) {
        sh.setUniform('u_res', [w, h]);
      }
      if (fs.match(/uniform\s+float\s+u_time/)) {
        sh.setUniform('u_time', sketchTime);
      }
      if(fs.match(/uniform\s+vec2\s+u_tracking/)){
        // target - currPos
         let x = p.mouseX.clamp(0, w);
        let y = p.mouseY.clamp(0, h);

        let delta = [(x/w) - tracking[0],
                     (y/h) - tracking[1]];
        tracking = [tracking[0] + delta[0] * easing,
                    tracking[1] + delta[1] * easing];


        sh.setUniform('u_tracking', tracking);
      }

      // TODO: Add for loop here
      if (fs.match(/uniform\s+sampler2D\s+u_texture0/)) {
        sh.setUniform('u_texture0', img0);
      }

      if (fs.match(/uniform\s+vec3\s+u_mouse/)) {
        let x = p.mouseX.clamp(0, w);
        let y = p.mouseY.clamp(0, h);
        sh.setUniform('u_mouse', [x, y, mouseIsDown]);
      }

      if (fs.match(/uniform\s+vec2\s+u_lastMouseDown/)) {
        sh.setUniform('u_lastMouseDown', lastMouseDown);
      }
      // sh.setUniform('u_lastMouseDown', lastMouseDown);

      p.quad(-1, -1, 1, -1, 1, 1, -1, 1);
    };
  };
  return sketch;
}


/*
    Fill all the textareas with glsl shader code.
    This keeps the html files smaller and makes the
    glsl code more maintainable since there will only be
    one place for each example.
*/
(function populateTextAreas() {

  /*
      glsl-code - will have an assigned sketch canvas
      js-code - for js code, so no canvas
      glsl-snippet - not a complete shader, no canvas
  */
  let arr = Array.from($('.glsl-code,.js-code,.glsl-code-snippet,.glsl-snippet'));

  arr.forEach(t => {
    let path = $(t).attr('data-example');
    let strParams = $(t).attr('data-params');
    let params = strParams ? JSON.parse(strParams) : {};

    // If we have a textarea without a path, it means that textarea
    // only has some inline code that doesn't require CodeMirror
    if (!path) {
      CodeMirror.fromTextArea(t, {
        lineNumbers: false,
        readOnly: true
      });
      return;
    }

    // let relPath = '../' + path;
    let relPath = path;

    fetch(relPath)
      .then(res => res.text())
      .then(fragShaderCode => {
        let fs = t.innerHTML = fragShaderCode;

        // If it's a glsl example, add the rendered result:
        // Get the div immediately following the textarea,
        // this is where we'll load the sketch
        // But p5 expects it to have to have an ID, so assign it one.

        // Other snippets are for js or vert shaders
        // that we don't want to make sketches for

        // If we have a glsl-code AND it should render
        // we'll need to build up some meta data stuff.
        if ($(t).hasClass('glsl-code') && (params && params.render !== 'false')) {

          // Will contain the glsl CodeMirror and the canvas
          let divContainer = $('<div>')
            .insertAfter(t)
            .addClass('lazy')
            .attr({
              'id': relPath,
              'data-lys-code': fs,
              'data-lys-relPath': relPath,
              'data-lys-params': strParams,
              'data-loader': 'customLoaderName'
            });

          $(t).prependTo(divContainer);
        }


        let lines = true;
        if (params && params.CodeMirror && params.CodeMirror.lineNumbers) {
          lines = (params.CodeMirror.lineNumbers === 'true') ? true : false;
        }

        let cm = CodeMirror.fromTextArea(t, {
          lineNumbers: lines,
          readOnly: true
        });

        if (params.lines) {
          params.lines.forEach(l => {
            cm.addLineClass(l, null, 'line-highlight');
          });
        }
      })
      .then(() => {
        // Lazy loading canvases
        $(function() {
          $('.lazy').lazy({
            customLoaderName: function(el) {
              let fragCode = el.attr('data-lys-code');
              let strParams = el.attr('data-lys-params');
              let relPath = el.attr('data-lys-relPath');
              let params = strParams ? JSON.parse(strParams) : {};

              let test = new p5(makeSketch(fragCode, params), relPath);
              let target = $(test.canvas).parent().parent().parent().find('#target');
              $(test.canvas).appendTo(target);
            }
          });
        });
      });
  });
})();