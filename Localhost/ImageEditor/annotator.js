// Copyright 2009  LLC. All Rights Reserved.

/**
 * @fileoverview This file consists of functions for accessing the common features of the editor like movement of mouse and drawing 
 * of shapes lines,use of pencil, color selection, handling of menu bar items and resizing of the canvas
 * The color selection depends on the file @jscolor.js 
 * @author prabhakar.ranjan@.com (Prabhakar Ranjan)
 */


// Declaration of Global variables
var  drawingPanel, canvas, context, canvastrans, contexttrans,canvasrev, contextrev;
// Declaration of interface object literals with properties as resizing and status
var interface = 
{
	resizing:false, 
	status:null
}
window.onload = function() { 	
// Initialization of values for tools and stroke capture on the canvas on loading of the image editor window.
	drawingPanel = document.getElementById('drawpanel');
	canvas = document.getElementById('canvas'); //The primary drawing Canvas.
	if(canvas.getContext) {
		context = canvas.getContext("2d");//Get context as 2-dimensional drawing on the canvas	
		context.lineWidth = 1;//Defaults values
		context.strokeStyle = '#000';
		var x= document.getElementById('textsize');
		context.fillStyle = '#FFF';
		context.strokeFill = 1; //Outer boundaries of geometries
		context.fillRect(0, 0, canvas.width, canvas.height); //Fill with intial background color of canvas
		context.tool = new tool.pencil();
		canvastrans = document.getElementById("canvastrans");
		contexttrans = canvastrans.getContext("2d");	
		canvasrev = document.getElementById("canvasrev");//This canvas is used to save images for undo.
		contextrev = canvasrev.getContext("2d");
		window.onmouseup = bodyUp;//Required for resizing the canvas manually
		window.onmousemove = bodyMove;//Required for resizing the canvas manually
   		canvas.onmousedown = canvastrans.onmousedown = mouseDown;
		canvas.onmousemove = canvastrans.onmousemove = mouseMove;
 		canvas.onmouseout = canvastrans.onmouseout = mouseOut;  // Mouse events for canvas and transition canvas should be same
		canvas.onmouseup = canvastrans.onmouseup = mouseUp;
		} 
}

/**
 * This function handles the resizing of the canvus when the canvastrans and canvasrev are to be used.
 * @param {width,height} Takes the width and the height of the canvas.
 * @return {null}. The function uses the methods of canvas to save and restore the previous states.
 */
function extendResize(w, h) { 
	undoSave();
	contextrev.fillStyle = context.fillStyle; //Save the current state
	canvas.width = canvastrans.width = w;
	canvas.height = canvastrans.height= h;
	canvas.style.width = canvastrans.style.width = w+'px';
	canvas.style.height = canvastrans.style.height = h+'px';
	var cresizer = document.getElementById('canvasresize');
	cresizer.style.left = w+cresizer.offsetWidth+'px'; 
	cresizer.style.top = h+cresizer.offsetHeight+'px';
	context.fillStyle = contextrev.fillStyle; //Restore to the saved state
	context.fillRect(0, 0, canvas.width, canvas.height); //Fills with background color
	context.drawImage(canvasrev, 0, 0);
}
/**
 * This function returns the postion of the object
 * @param {object} Takes the object as the parameter.
 * @return {x,y}. Returns the mouse position of the object.
 */
function mousePos(obj) {
	
	var x = y = 0;
	var x, y;
	x = e.layerX;
    y = e.layerY;							
	return { x:x, y:y }
}
/**
 * This function returns the mouse coordinates relative to the object
 * @param {event,object} Takes the event and object as the parameter.
 * @return {x,y}. Returns the mouse coordinates relative to the object.
 */
function mouseCoords(e, obj) {
          	if(context) {
		var x, y;
		x = e.layerX;
        y = e.layerY;														
		return { x: x-.5, y: y-.5 }; //To remove the aliasing effect(.5 subtracted)
	}
}
// Declaration of constructors for tools to be used from the toolbar
var tool = {
	Shapes: function() {
        undoSave();
		this.down = this.mouse_down = function() {
			actvateTransitioncanvas();
			this.start = { x:m.x, y:m.y } 
			this.status = 1;
			context.beginPath();
		}
		this.mouse_move = function() {
			contexttrans.clearRect(0, 0, canvastrans.width, canvastrans.height);
		}
		this.mouse_up = function() {
			canvastrans.style.display='none';
			this.status = 0;
		}
	},

	Pencil: function() {
        this.down = function() {
			this.last = null;
			this.disconnected = null;
			context.beginPath();
			undoSave();
			this.status = 1;
		}
		this.move = function(e) { 

			if(this.disconnected) {	//For nothing to be drawn when moving back to canvas
				this.disconnected = null;
				this.last = { x:m.x, y:m.y }
			} else {				
				this.draw();
			}
			context.moveTo(m.x, m.y);
	}
		this.up = function() {
			this.status = 0;
		}
		this.draw = function() {
                context.lineTo(m.x, m.y);
				context.stroke();	
				context.beginPath();
				this.last = { x:m.x, y:m.y }	
		}
	},
        pencil: function() {
        this.name = 'pencil';
		this.status = 0;
		this.inherit = tool.Pencil; 
		this.inherit();
        context.lineCap = 'round';
		context.lineWidth = 1;
	},
        eraser: function() {
        this.name = 'eraser';
		this.status = 0;
		this.inherit = tool.Pencil;
		this.inherit();
		context.lineCap = 'square';
		context.lineWidth = 7;
		context.strokeStyle = '#FFF'; 
	},
    	line: function() {
		this.name = 'line';
		this.status = 0;
        this.inherit = tool.Shapes; 
		this.inherit();
    	context.lineCap = 'round';
		context.lineWidth = 1;
        this.move = function(e) {
			this.mouse_move();
			drawLine(this.start.x, this.start.y, m.x, m.y, contexttrans);
		}
		this.up = function(e) {
			this.mouse_up();
			drawLine(this.start.x, this.start.y, m.x, m.y, context);
		}
	},
    	rectangle: function() {
		this.name = 'rectangle';
		this.status = 0;
		this.inherit = tool.Shapes; 
		this.inherit();
		this.move = function(e) {
			this.mouse_move();
			drawRectangle(this.start.x, this.start.y, m.x, m.y, contexttrans);
		}
		this.up = function(e) {
			this.mouse_up();
			drawRectangle(this.start.x, this.start.y, m.x, m.y, context);
		}
	},
    	circle: function() {
    	this.name = 'circle';
		this.status = 0;
		this.inherit = tool.Shapes; 
		this.inherit();
    	this.down = function(e) {
			this.mouse_down();
			this.lastLineWidth = context.lineWidth;
		}
		this.move = function(e) {
			this.mouse_move();
			drawCircle(this.start.x, this.start.y, m.x, m.y, contexttrans);
		}
		this.up = function(e) {
			this.mouse_up();
			drawCircle(this.start.x, this.start.y, m.x, m.y, context);
		}
    },
};
/**
 * This function handles the onmousedown event of mouse on the canvas.
 * @param {event} .
 */

function mouseDown(e) {
var source = e.currentTarget;
	m = mouseCoords(e, canvas);
	context.tool.down(e);
	context.moveTo(m.x, m.y); 
    return false;
}
/**
 * This function handles the onmouseup event of mouse on the canvas.
 * @param {event} .
 */
function mouseUp(e) {
	m = mouseCoords(e, canvas);
	e.stopPropagation();
	if(interface.resizing || context.resizing) { bodyUp(e); } 
	context.tool.up(e);
		if(context.tool.name != 'eraser' ) { //No color switching
		if(e.button == 2 && context.tool.name != 'eraser') { //Switching stroke & fill 
			var temp = context.fillStyle;
			context.fillStyle = context.strokeStyle;
			context.strokeStyle = temp;
		}
	}
	return false;
}
/**
 * This function handles the movement of mouse on the canvas.
 * @param {event} .
 */
    function mouseMove(e) {
	m = mouseCoords(e, canvas);
	e.stopPropagation();
	if(interface.resizing || context.resizing) { bodyMove(e); } //Handles movement when resizing is performed.
	if(context.tool.status > 0) {
		context.tool.move(e);
	}
	return false;
}
/**
 * This function handles the movement of mouse outside the canvas.
 * @param {event} .
 * @return {null}
 */
    function mouseOut(e) {
	if(context && (context.tool.name=='pencil' || context.tool.name=='eraser') && context.tool.status==1) { 
		context.tool.disconnected = 1;
		m = mouseCoords(e, canvas);
		context.tool.draw();
	}
}
/**
 * This function clears and shows the transition canvas.
 * Uses the canvas - canvastrans for this purpose.
 */
function actvateTransitioncanvas() {
	if(m) { contexttrans.moveTo(m.x, m.y); }//Copy context from main to temporary canvas
	contexttrans.lineCap = context.lineCap;	//Swapping of values between main canvas and temporary canvas							
	contexttrans.lineWidth = context.lineWidth;
	contexttrans.strokeStyle = context.strokeStyle;
	contexttrans.fillStyle = context.fillStyle;
	contexttrans.clearRect(0, 0, canvastrans.width, canvastrans.height);	//Clear temporary canvas
	canvastrans.style.display='block';	  //Show temporary canvas
}
/**
 * This function restores the canvas to a previous state.
 * Uses the canvas - canvasrev to save a current state on the canvas.
 */
function undoSave() {                                          
	if(canvas.width != canvasrev.width || canvas.height != canvasrev.height) { 
		canvasrev.width = canvas.width;
		canvasrev.height = canvas.height;
	}
	contextrev.drawImage(canvas, 0, 0);
}
/**
 * This function restores the canvas to a previous state.
 * Uses the canvas - canvasrev for restoring.
 */
function undoLoad() {
if(canvas.width != canvasrev.width || canvas.height != canvasrev.height){
	 	extendResize(canvasrev.width, canvasrev.height);           
	}
	contexttrans.drawImage(canvas, 0, 0);
	context.drawImage(canvasrev, 0, 0);
	contextrev.drawImage(canvastrans, 0, 0);
}
/**
 * This function handles the resizing of window.
 * @param {event,object} .
 * @return {null} .
 */
function resizeWindow(e, obj) {
	interface.resizing = true;
	var pos = mousePos(obj);
	context.start = { x:obj.offsetWidth+3-(e.clientX-pos.x), y:obj.offsetHeight+3-(e.clientY-pos.y) };
	//Retrieves the width of the object relative to the layout
	e.stopPropagation();//Provides the event to not to bubble up the Document Object Model (DOM) hierarchy
	document.body.style.cursor = 'nw-resize';
}
/**
 * This function handles the resizing of canvas.
 * @param {event} .
 * @return {null}.
 */
function canvasResize(e) {
	context.resizing = true;
	document.body.style.cursor = 'nw-resize';
	canvastrans.lastCursor = canvastrans.style.cursor;
	canvastrans.style.cursor = 'nw-resize';
	actvateTransitioncanvas();
}
/**
 * This function handles the movement of mouse for drawing on the canvas and allows user to move outside the canvas.
 * @param {event} .
 * @return {null}.
 */
function bodyMove(e) {
	if(context.tool.status > 0) { mouseMove(e); }	
	if(context.resizing) {	
		m = mouseCoords(e, document.body);
		contexttrans.clearRect(0, 0, canvastrans.width, canvastrans.height);
		contexttrans.strokeRect(0, 0, m.x, m.y); //dotted line
		} 
	else if(interface.resizing) {
		var win = drawingPanel.parentNode.parentNode.parentNode;
		m = mouseCoords(e, document.body);
		win.style.width = e.clientX-win.offsetLeft+context.start.x+3+'px';
		win.style.height = e.clientY-win.offsetTop+context.start.y+3+'px';
	}
}
/**
 * This function handles the closing of menus if mouse moved outside the body and prevents drawing on the canvas in such a case.
 * @param {event} .
 * @return {null}.
 */
function bodyUp(e) {
	if(context.resizing) {
		context.resizing = false; 
		document.body.style.cursor = 'auto';
		canvastrans.style.cursor = canvastrans.lastCursor;
		m = mouseCoords(e, drawingPanel);
		extendResize(m.x-3, m.y-3);
	}
	if(interface.resizing) { interface.resizing = false; document.body.style.cursor = 'auto'; }
    if(document.getElementById('menubar').className=='open') {
		document.getElementById('menubar').className='';
		e.stopPropagation();
	}
}
/**
 * This function handles the drawing of lines on the canvas.
 * @param {x1,y1,x2,y2,context} 
 * (x1,y1) are the coordinates of the starting point.
 * (x2,y2) are the coordinates of the end point.
 * context parameter is the context(two-dimensional) to be used for drawing the lines.
 * @return Returns the line drawn on the canvas.
 * Uses mathematical methods and methods used for drawing lines on canvas.
 */
function drawLine(x1, y1, x2, y2,context1) { 
	if(context1.lineWidth % 2 == 0) { x1 = Math.floor(x1); y1 = Math.floor(y1); x2 = Math.floor(x2); y2 = Math.floor(y2); } 
	//Remove aliasing effect
	context1.beginPath();
	context1.moveTo(x1, y1);
	context1.lineTo(x2, y2);
	context1.stroke();
	context1.beginPath();
	return { x:x2, y:y2 }
}
/**
 * This function handles the drawing of circles on the canvas.
 * @param {x1,y1,x2,y2,context} 
 * (x1,y1) are the coordinates of the starting point.
 * (x2,y2) are the coordinates of the end point.
 * context parameter is the context(two-dimensional) to be used for drawing the circle.
 * @return Returns the circle drawn on the canvas.
 * Uses mathematical methods and methods used for drawing arcs on canvas.
 */
function drawCircle(x1, y1, x2, y2, context1) {
		var centerX = Math.max(x1,x2) - Math.abs(x1 - x2)/2;
		var centerY = Math.max(y1,y2) - Math.abs(y1 - y2)/2;
		context1.beginPath();
		var distance = Math.sqrt(Math.pow(x1 - x2,2) + Math.pow(y1 - y2,2));
		//Method to draw arcs
		context1.arc(centerX, centerY, distance/2,0,Math.PI*2 ,true); 
		context1.stroke();
		context1.closePath();
}
/**
 * This function handles the drawing of rectangles on the canvas.
 * @param {x1,y1,x2,y2,context} 
 * (x1,y1) are the coordinates of the starting point.
 * (x2,y2) are the coordinates of the end point.
 * context parameter is the context(two-dimensional)to be used for drawing the rectangle.
 * @return Returns the rectangle drawn on the canvas.
 * Uses methods used for stroking of rectangles on canvas.
 */
function drawRectangle(x1, y1, x2, y2,context1) {
    if(context.strokeFill == 2 || context1.lineWidth % 2 == 0) {    //Remove aliasing effect
		x1 = Math.floor(x1); y1 = Math.floor(y1); x2 = Math.floor(x2); y2 = Math.floor(y2);
	}
	context1.strokeRect(x1, y1, (x2-x1), (y2-y1));
}
/**
 * This function handles the selection of tools from the toolbar.
 * @param {object} .
 * @return {tool} Returns the tool to be used.
 */
 function selectTool(obj) {
    document.getElementById("txt").value ='';//Prevents text to be written if focus is on other tools
	context.tool.status = 0;
	canvastrans.style.display='none';
	var newtool = obj.id;
	document.getElementById('drawpanel').className = newtool;
    //Highlight buttons
	var toolbarbtns = document.getElementById('buttons').getElementsByTagName('li');
	for(var i=0; i<toolbarbtns.length;i++) {
		if(toolbarbtns[i].className == 'sel') { toolbarbtns[i].className=''; }
	}
	obj.className = 'sel';
	if(context.lastStrokeStyle) //Reset color 
	{ 
	selectColor(context.lastStrokeStyle); 
	context.lastStrokeStyle = null; 
	}
    context.lastTool = context.tool.name;
	context.tool = new tool[newtool]();
    var newtool = shareSettingsPanels(context.tool.name);
    var settingpanels = document.getElementById('settings').getElementsByTagName('div');
	for(var i=0; i<settingpanels.length;i++) {
		if(settingpanels[i].style.display == 'block') 
		{ 
		settingpanels[i].style.display='none'; 
		}
	}
	
				if(document.getElementById(newtool+'-settings')) {
		document.getElementById(newtool+'-settings').style.display = 'block';
			var settingbtns = document.getElementById(newtool+'-settings').childNodes;
			for(var i=0; i<settingbtns.length;i++) { //reapply last selection
				if(settingbtns[i].className == 'sel') 
				{ 
				settingbtns[i].onclick(); 
				}
			}
		}
	}
/**
 * This function handles the selection of settings as width of outlines for the different tools .
 * @param {object,setting} .
 * @return {tool} Returns the settings to be applied for any tool.
 */
function selectSetting(obj, sett) {
	context.tool.status = 0;
	canvastrans.style.display='none';
	var newtool = shareSettingsPanels(context.tool.name);
		if(document.getElementById(newtool+'-settings')) {
		var settingbtns = document.getElementById(newtool+'-settings').childNodes;
		for(var i=0; i<settingbtns.length;i++) {
			if(settingbtns[i].className == 'sel') 
			{ 
			settingbtns[i].className=''; 
			}
		}
		obj.className = 'sel';
		eval(sett);//Executes the settings argument.
	}    
}
/**
 * This function allows the selection of colors 
 * @param {Object,Event and Context} 
 * @return {col} Returns the color to be used as background color for the object under consideration.
 */ 
function selectColor(obj, e, conText) {
	col = (typeof(obj) == 'string') ? obj : obj.style.backgroundColor;
    selectColor2(col, e, conText);
}
/**
 * This function allows the application of colors when tools are intechanged
 * @param {Color, Event and Context} 
 * @return {col} Returns the color to be used as strokeStyle of the current context.
 */
function selectColor2(col, e, conText) {
	if(e && e.ctrlKey) {	
    	context.strokeStyle = col;
	} else if(conText == 1 || (e && e.button == 2)) { 
		context.fillStyle = col;
		contexttrans.fillStyle=col;
	} 
	else 
	{
		context.strokeStyle=col;
		contexttrans.strokeStyle=col;
		if(context.lastStrokeStyle) //This allows color change when eraser and other tools are used.
		{ 
		context.lastStrokeStyle = col; 
		} 
	}
  	if(e) e.preventDefault();
}

/**
 * This function handles the application of settings as width of outlines for the tools .
 * @param {tool} .
 * @return {tool} Returns the settings to be applied for the tool selected.
 */
function shareSettingsPanels(tool) {
	if(tool=='line' ||tool=='rectangle' || tool=='circle' || tool=='pencil' ) 
	{ 
	return 'line'; 
	}
	return tool;
}
/**
 * This function handles the click on a button in toolbar .
 * @param {event,object} .
 * @return Returns the down style to be applied on tool selected.
 */
function buttonDown(e, obj) {
	if(e.button != 2 && obj.className != 'sel') 
	{ 
	obj.className='down'; 
	} 
}
/**
* This function is used to convert the image drawn on the canvas to the Base64.
* @return {dataurl} The toDatURL method is used to get the Base64 string for the image.
*/
function GetHiddenFieldValue(txt1) {

    var dataurl = canvas.toDataURL();
    document.getElementById(txt1).value = dataurl;
}

function init() { // For intializing the canvas.
    canvas = document.getElementById("canvas");
}

/**
* The method used below associates a function with a particular event and binds the event to the current node. addEventListener()  *
* accepts the following 3 parameters:
* EventType: A string representing the event to bind, without the "on" prefix. Here it is load event.
* listener: The object or function to fire when the event fires. The actual parameter entered should be a reference to the function or * object.Here it is "init"
* useCapture:  Set to true or false, respectively.
*/
window.addEventListener("load", init, true);
       
 
