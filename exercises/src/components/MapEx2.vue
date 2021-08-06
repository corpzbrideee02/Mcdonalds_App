<template>
  <div>
    <div class="heading">TomTom Map Ex2</div>
    <div class="status">show map</div>
    <div>
      <InputText
        placeholder="enter current address"
        id="address"
        v-model="state.address"
      />
      <br />
    </div>
    <div style="margin: 3vh">
      <Button
        label="Gen Map"
        @click="genMap"
        class="p-button-outlined"
        style="width: 30vw"
      />
    </div>
    <div id="map" ref="mapRef" v-show="state.showmap === true"></div>
  </div>
</template>
<script>
import { ref, reactive } from "vue";
export default {
  name: "Map",
  setup() {
    const mapRef = ref(null);
    let state = reactive({
      status: "",
      address: "",
      showmap: false,
    });
    const genMap = async () => {
      try {
        state.showmap = true;
        const tt = window.tt;
        let url = `https://api.tomtom.com/search/2/geocode/${state.address}.json?key=HZTNMJGyvobd3HhGaZd14rGnpLPwv0mL`;
        let response = await fetch(url);
        let payload = await response.json();
        let lat = payload.results[0].position.lat;
        let lon = payload.results[0].position.lon;
        let map = tt.map({
          key: "HZTNMJGyvobd3HhGaZd14rGnpLPwv0mL",
          container: mapRef.value,
          source: "vector/1/basic-main",
          center: [lon, lat],
          zoom: 8,
        });
        map.addControl(new tt.FullscreenControl());
        map.addControl(new tt.NavigationControl());
      } catch (err) {
        state.status = err.message;
      }
    };
    return {
      mapRef,
      state,
      genMap,
    };
  },
};
</script>
<style>
#map {
  height: 50vh;
  width: 90vw;
  margin-left: 3vw;
  border: solid;
}
#popup {
  font-weight: bold;
}
</style>
