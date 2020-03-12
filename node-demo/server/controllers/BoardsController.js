import express from "express";
import BaseController from "../utils/BaseController";
import { boardsService } from "../services/BoardsService";
import auth0Provider from "@bcwdev/auth0Provider";

export class BoardsController extends BaseController {
  constructor() {
    super("api/Boards");
    this.router = express
      .Router()
      .use(auth0Provider.getAuthorizedUserInfo)
      .get("", this.get)
      // NOTE: Beyond this point all routes require Authorization tokens (the user must be logged in)
      .use(auth0Provider.hasPermissions("create:board"))
      .post("", this.create)
      .use(auth0Provider.hasPermissions("delete:board"))
      .delete("/:id", this.deleteValue);
  }
  async get(req, res, next) {
    try {
      let boards = await boardsService.find({ ...req.query, creatorEmail: req.userInfo.email })
      return res.send(boards);
    } catch (error) {
      next(error);
    }
  }
  async create(req, res, next) {
    try {
      // NOTE NEVER TRUST THE CLIENT TO ADD THE CREATOR ID
      req.body.creatorEmail = req.userInfo.email;
      let board = await boardsService.create(req.body);
      res.send(board);
    } catch (error) {
      next(error);
    }
  }

  async deleteValue(req, res, next) {
    res.send(true);
  }
}
