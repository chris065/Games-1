function mouseDownListener(evt) {
    var mousePos = getMousePos(canvas, evt);

    // The index of the tile being clicked or -1 if no tile was clicked
    var dragIndex = getDragIndex(mousePos.X, mousePos.Y);

    if (dragIndex > -1) {
        isDragging = true;
        wasDragged = false; // not yet
        window.addEventListener("mousemove", mouseMoveListener, false);

        dragTile = tiles[dragIndex];
        dragTile.isMoving = true;

        // We now place the currently dragged tile on top by placing it last in the array.
        tiles.push(tiles.splice(dragIndex, 1)[0]);

        // We record the point on the dragged tile where the mouse is "holding" it:          
        dragTile.clickOffsetX = mousePos.X - dragTile.X;
        dragTile.clickOffsetY = mousePos.Y - dragTile.Y;

        // The "target" position is where the object should be if it were to move there instantaneously. But we will
        // set up the code so that this target position is approached gradually, producing a smooth motion. 
        dragTile.targetPosX = dragTile.X;
        dragTile.targetPosY = dragTile.Y;

        // Start timer
        timer = setInterval(onTimerTick, 1000 / 60);
    }
    canvas.removeEventListener("mousedown", mouseDownListener, false);
    window.addEventListener("mouseup", mouseUpListener, false);

    // Prevents the mouse down from having an effect on the main browser window:
    evt.preventDefault();
}

function mouseUpListener(evt) {
    canvas.addEventListener("mousedown", mouseDownListener, false);
    window.removeEventListener("mouseup", mouseUpListener, false);
    if (isDragging) {
        isDragging = false;

        if (wasDragged) {
            // Make the tile return to its intial position
            dragTile.targetPosX = dragTile.initalX;
            dragTile.targetPosY = dragTile.initalY;
        } else {
            // Make the tile go up
            dragTile.targetPosX = 0;
            dragTile.targetPosY = 0;
            addToWord(dragTile.text);

            wordHolder.addTile(dragTile);
        }
        window.removeEventListener("mousemove", mouseMoveListener, false);
    }
}

function mouseMoveListener(evt) {
    // Updates target position
    var minX = 0;
    var maxX = canvas.width - dragTile.size;
    var minY = 0;
    var maxY = canvas.height - dragTile.size;

    var mousePos = getMousePos(canvas, evt);

    // Clamp x and y positions to prevent object from dragging outside of canvas
    dragTile.targetPosX = Math.min(Math.max(mousePos.X - dragTile.clickOffsetX, minX), maxX);
    dragTile.targetPosY = Math.min(Math.max(mousePos.Y - dragTile.clickOffsetY, minY), maxY);
}

// Returns the index of the tile being clicked or -1 if no tile was clicked
function getDragIndex(mouseX, mouseY) {
    var dragIndex = -1;
    // the variable will be overwritten to ensure only the topmost tile is dragged
    for (var i = 0; i < tiles.length; i++)
        if (tiles[i].isClicked(mouseX, mouseY))
            dragIndex = i;
    return dragIndex;
}

// Translates the mouse position to canvas coodrinates
function getMousePos(canvas, evt) {
    var bRect = canvas.getBoundingClientRect();
    return {
        X: (evt.clientX - bRect.left) * (canvas.width / bRect.width),
        Y: (evt.clientY - bRect.top) * (canvas.height / bRect.height)
    };
}

// Runs while the timer is ticking
function onTimerTick() {
    // The next variable controls the lag in the tile movement (from 0 to 1)
    var easeAmount = 0.2;
    // Update the moving tile position
    dragTile.X += easeAmount * (dragTile.targetPosX - dragTile.X);
    dragTile.Y += easeAmount * (dragTile.targetPosY - dragTile.Y);

    if ((Math.abs(dragTile.X - dragTile.initalX) > 5) || (Math.abs(dragTile.Y - dragTile.initalY) > 5)) {
        wasDragged = true;
    }

    // Stop the timer when the target position is reached (close enough)
    if ((!isDragging) && (Math.abs(dragTile.X - dragTile.targetPosX) < 0.1) && (Math.abs(dragTile.Y - dragTile.targetPosY) < 0.1)) {
        // Snap the tile to its final position
        dragTile.X = dragTile.targetPosX;
        dragTile.Y = dragTile.targetPosY;

        dragTile.isMoving = false;
        wasDragged = false;
        // Stop timer:
        clearInterval(timer);
    }
    drawScreen();
}