<template>
  <div class="home">
    <img alt="Vue logo" src="../assets/logo.png" />
    <h1>Welcome to Your Vue.js App</h1>
    <div v-if="$auth.isAuthenticated">
      <img :src="$auth.user.picture" height="45" alt />
      {{$auth.user.name}}
    </div>
    <div>
      <button @click="getBlogs">GET BLOGS</button>
      <h4>Blogs</h4>
      {{blogs}}
    </div>

    <div>
      <button @click="getBoards">GET Boards</button>
      <button @click="createBoard">CREATE Boards</button>
      <h4>Boards</h4>
      {{boards}}
    </div>
  </div>
</template>

<script>
export default {
  name: "home",
  data() {
    return {
      blogs: [],
      boards: []
    };
  },
  methods: {
    async getBlogs() {
      try {
        let res = await fetch("https://localhost:5001/api/blogs", {
          headers: {
            authorization: this.$auth.bearer
          }
        });
        let blogs = await res.json();
        this.blogs = blogs;
      } catch (e) {
        alert(e);
      }
    },
    async createBoard() {
      try {
        let res = await fetch("http://localhost:3000/api/boards", {
          method: "POST",
          headers: {
            authorization: this.$auth.bearer
          },
          body: JSON.stringify({
            title: "MY BOARD",
            description: "AUTH0 is neat"
          })
        });
        let boards = await res.json();
        this.boards = boards;
      } catch (e) {
        alert(e);
      }
    },
    async getBoards() {
      try {
        let res = await fetch("http://localhost:3000/api/boards", {
          headers: {
            authorization: this.$auth.bearer
          }
        });
        let boards = await res.json();
        this.boards = boards;
      } catch (e) {
        alert(e);
      }
    }
  }
};
</script>
