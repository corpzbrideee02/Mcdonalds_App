<template>
  <div>
    <div class="heading">TomTom Map Ex1</div>
    <div class="status">get lat and lon</div>
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
        label="Get Lat/Lon"
        @click="getLatLon"
        class="p-button-outlined"
        style="width: 30vw"
      />
    </div>
    <p></p>
    {{ state.latlon }}
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
      latlon: "",
    });
    const getLatLon = async () => {
      try {
        let url = `https://api.tomtom.com/search/2/geocode/${state.address}.json?key=HZTNMJGyvobd3HhGaZd14rGnpLPwv0mL `;
        let response = await fetch(url);
        let payload = await response.json();
        let lat = payload.results[0].position.lat;
        let lon = payload.results[0].position.lon;
        state.latlon = `Address is at Lat ${lat} and Lon ${lon} `;
      } catch (err) {
        state.status = err.message;
      }
    };
    return {
      mapRef,
      state,
      getLatLon,
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
