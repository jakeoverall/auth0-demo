import BoardSchema from "../models/Board";
import mongoose from "mongoose";

class DbContext {
  Boards = mongoose.model("Boards", BoardSchema);
}

export const dbContext = new DbContext();
