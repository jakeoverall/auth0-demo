import { dbContext } from "../db/DbContext";
import { BadRequest } from "../utils/Errors";

class BoardsService {
  async create(board) {
    return await dbContext.Boards.create(board);
  }
  async find(query = {}) {
    let boards = await dbContext.Boards.find(query);
    return boards;
  }
  async findById(id) {
    let value = await dbContext.Boards.findById(id);
    if (!value) {
      throw new BadRequest("Invalid Id");
    }
    return value;
  }
}

export const boardsService = new BoardsService();