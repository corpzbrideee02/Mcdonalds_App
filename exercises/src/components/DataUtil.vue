<template>
  <div>
    <div class="heading">Data Utility</div>
    <div class="status">{{ state.status }}</div>
    <Button
      label="Load Categories"
      @click="resetTables"
      class="p-button-outlined"
    />&nbsp;
    <Button label="Load Stores" @click="loadStores" class="p-button-outlined" />
  </div>
</template>
<script>
import { reactive } from "vue";
import { fetcher } from "../util/apiutil";
export default {
  setup() {
    let state = reactive({
      status: "",
    });
    const resetTables = async () => {
      let url = "https://localhost:44371/api/Data";
      try {
        state.status = "resetting tables ...";
        let payload = await fetcher("Data");
        state.status = payload;
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };
    const loadStores = async () => {
      try {
        state.status = "loading store data ...";
        let payload = await fetcher("Data/loadstores");
        state.status = payload;
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };
    return {
      resetTables,
      loadStores,
      state,
    };
  },
};
</script>
