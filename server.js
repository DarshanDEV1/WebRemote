const express = require('express');
const http = require('http');
const path = require('path');
const socketio = require('socket.io');
const cors = require('cors');

const app = express();
const server = http.createServer(app);
const io = socketio(server, {
    cors: {
        origin: "*",
        methods: ["GET", "POST"]
    }
});

let latestSignal = '';

app.use(cors());
app.use(express.static(path.join(__dirname, 'public')));

io.on('connection', (socket) => {
    console.log('New client connected');

    socket.on('action_signal', (data) => {
        console.log('Received action:', data.signal);
        latestSignal = data.signal;
    });

    socket.on('disconnect', () => {
        console.log('Client disconnected');
    });
});

app.get('/get-latest-signal', (req, res) => {
    res.json({ signal: latestSignal });
    latestSignal = '';  // Clear the signal after sending it
});

const PORT = process.env.PORT || 3000;
server.listen(PORT, () => console.log(`Server running on port ${PORT}`));
