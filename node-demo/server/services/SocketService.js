import SocketIO from "socket.io";
import auth0Provider from "@bcwdev/auth0Provider";
class SocketService {
  io = SocketIO();
  /**
   * @param {SocketIO.Server} io
   */
  setIO(io) {
    try {
      this.io = io;
      //Server listeners
      io.on("connection", this._onConnect());
    } catch (e) {
      console.error("[SOCKETSTORE ERROR]", e);
    }
  }

  /**
   * @param {SocketIO.Socket} socket
   */
  async Authenticate(socket, bearerToken) {
    try {
      let user = await auth0Provider.getUserInfoFromBearerToken(bearerToken);
      socket["user"] = user;
      socket.join(user.id);
      socket.emit("AUTHENTICATED");
      this.io.emit("UserConnected", user.id);
    } catch (e) {
      socket.emit("error", e);
    }
  }

  /**
   * @param {SocketIO.Socket} socket
   * @param {string} room
   */
  JoinRoom(socket, room) {
    socket.join(room);
  }
  /**
   * @param {SocketIO.Socket} socket
   * @param {string} room
   */
  LeaveRoom(socket, room) {
    socket.leave(room);
  }

  /**
   * Sends a direct message to a user
   * @param {string} userId
   * @param {string} eventName
   * @param {any} payload
   */
  messageUser(userId, eventName, payload) {
    try {
      this.io.to(userId).emit(eventName, payload);
    } catch (e) {}
  }

  messageRoom(room, eventName, payload) {
    this.io.to(room).emit(eventName, payload);
  }

  _onConnect() {
    return socket => {
      this._newConnection(socket);

      //STUB Register listeners

      socket.on("dispatch", this._onDispatch(socket));
      socket.on("disconnect", this._onDisconnect(socket));
    };
  }

  _onDisconnect(socket) {
    return () => {
      try {
        if (!socket.user) {
          return;
        }
        this.io.emit("UserDisconnected", socket.user.id);
      } catch (e) {}
    };
  }

  _onDispatch(socket) {
    return (payload = {}) => {
      try {
        var action = this[payload.action];
        if (!action || typeof action != "function") {
          return socket.emit("error", "Unknown Action");
        }
        action.call(this, socket, payload.data);
      } catch (e) {}
    };
  }

  _newConnection(socket) {
    //Handshake / Confirmation of Connection
    socket.emit("CONNECTED", {
      socket: socket.id,
      message: "Successfully Connected"
    });
  }
}

const socketService = new SocketService();

export default socketService;
