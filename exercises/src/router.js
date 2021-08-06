import { createWebHistory, createRouter } from "vue-router";
import Home from "./components/Home.vue";
import CategoryList from "./components/CategoryList.vue";
import Tray from "./components/Tray.vue";
import Register from "./components/Register.vue";
import Login from "./components/Login.vue";
import Logout from "./components/Logout.vue";
import TrayList from "./components/TrayList.vue";
import MapEx3 from "./components/MapEx3.vue";
import Stores from "./components/Stores.vue";
import DataUtil from "./components/DataUtil.vue";
const routes = [
  { path: "/", name: "Home", component: Home },
  { path: "/categories", name: "Categories", component: CategoryList },
  { path: "/tray", name: "tray", component: Tray },
  { path: "/trays", name: "trays", component: TrayList },
  { path: "/register", name: "Register", component: Register },
  { path: "/login", name: "login", component: Login },
  { path: "/logout", name: "logout", component: Logout },
  { path: "/map",name: "map", component: MapEx3},
  { path: "/utility",name: "utility", component: DataUtil},
  { path: "/store",name: "store", component: Stores},
];
const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;

router.beforeEach((to, from, next) => {
  const publicPages = ["/login", "/register", "/logout","/map","/utility", "/store"];
  const authRequired = !publicPages.includes(to.path);
  if (authRequired && !sessionStorage.getItem("user")) {
    next({
      name: "login",
      params: { nextUrl: to.name },
    });
  } else {
    next();
  }
});
