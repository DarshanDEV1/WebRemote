const express = require('express');
const http = require('http');
const path = require('path');
const socketio = require('socket.io');
const { v4: uuidv4 } = require('uuid');

const app = express();
const server = http.createServer(app);
const io = socketio(server, {
    cors: {
        origin: "http://localhost:5500",
        methods: ["GET", "POST"]
    }
});

app.use(express.static(path.join(__dirname, 'public')));

const rooms = {};

app.post('/create_room', (req, res) => {
    const roomCode = uuidv4().substring(0, 8);
    rooms[roomCode] = [];
    res.json({ roomCode });
    console.log(`Room created with code: ${roomCode}`);
});

io.on('connection', (socket) => {
    console.log('New client connected');

    socket.on('join_room', (roomCode) => {
        if (rooms[roomCode]) {
            rooms[roomCode].push(socket.id);
            socket.join(roomCode);
            console.log(`Client joined room: ${roomCode}`);
            socket.emit('room_joined');
        } else {
            socket.emit('invalid_room');
        }
    });

    socket.on('action_signal', (data) => {
        const { signal, room } = data;
        if (rooms[room]) {
            io.to(room).emit('action_signal', signal);
        }
    });

    socket.on('disconnect', () => {
        console.log('Client disconnected');
        for (const room in rooms) {
            rooms[room] = rooms[room].filter(id => id !== socket.id);
            if (rooms[room].length === 0) {
                delete rooms[room];
            }
        }
    });
});

server.listen(3000, () => {
    console.log('Server listening on port 3000');
});
