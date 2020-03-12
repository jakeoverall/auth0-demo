<template>
  <div class="home">
    <img alt="Vue logo" src="../assets/logo.png" />
    <h1>Welcome to Your Vue.js App</h1>

    <div>
      <h4>Blogs</h4>
      {{blogs}}
    </div>

    <div>
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
        let res = await fetch("https://localhost:5001/api/blogs");
        let blogs = await res.json();
        this.blogs = blogs;
      } catch (e) {
        alert(e);
      }
    },
    async getBoards() {}
  }
};
</script>
