const express = require('express');
const http = require('http');
const path = require('path');
const socketio = require('socket.io');
const dgram = require('dgram');
const cors = require('cors');

const app = express();
const server = http.createServer(app);
const io = socketio(server, {
    cors: {
        origin: "*",
        methods: ["GET", "POST"]
    }
});

const unityUdpPort = 41234;
const unityUdpAddress = '127.0.0.1';
const udpClient = dgram.createSocket('udp4');

app.use(cors());
app.use(express.static(path.join(__dirname, 'public')));

io.on('connection', (socket) => {
    console.log('New client connected');

    socket.on('action_signal', (data) => {
        console.log('Received action:', data.signal);
        const message = Buffer.from(data.signal);
        udpClient.send(message, unityUdpPort, unityUdpAddress, (err) => {
            if (err) console.error('UDP message send error:', err);
        });
    });

    socket.on('disconnect', () => {
        console.log('Client disconnected');
    });
});

const PORT = process.env.PORT || 3000;
server.listen(PORT, () => console.log(`Server running on port ${PORT}`));
