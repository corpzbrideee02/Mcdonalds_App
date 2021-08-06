<template>
  <div>
    <div class="heading">3 Closest Stores</div>
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
        label="Find 3 Closest"
        @click="getLatLon"
        class="p-button-outlined"
        style="width: 30vw"
      />
    </div>
    <!-- <p></p>
    {{ state.latlon }} -->
    <div id="map" ref="mapRef" v-show="state.showmap === true"></div>
  </div>
</template>
<script>
import { ref, reactive } from "vue";

import { fetcher } from "../util/apiutil";
export default {
  name: "Map",
  setup() {
    const mapRef = ref(null);
    let state = reactive({
      status: "",
      address: "",
      latlon: "",
      showmap: false,
    });
    const getLatLon = async () => {
      try {
        state.showmap = true;
        const tt = window.tt;
        let url = `https://api.tomtom.com/search/2/geocode/${state.address}.json?key=HZTNMJGyvobd3HhGaZd14rGnpLPwv0mL `;
        let response = await fetch(url);
        let payload = await response.json();
        let lat = payload.results[0].position.lat;
        let lon = payload.results[0].position.lon;
       state.latlon = `Address is at Lat ${lat} and Lon ${lon} `;
        let map = tt.map({
          key: "HZTNMJGyvobd3HhGaZd14rGnpLPwv0mL",
          container: mapRef.value,
          source: "vector/1/basic-main",
          center: [lon, lat],
          zoom: 8,
        });
        map.addControl(new tt.FullscreenControl());
        map.addControl(new tt.NavigationControl());

        let threeStores = await fetcher(`store/${lat}/${lon}`);
        threeStores.map((store) => {
          let marker = new tt.Marker()
            .setLngLat([store.longitude, store.latitude])
            .addTo(map);
          let popupOffset = 25;
          let popup = new tt.Popup({ offset: popupOffset });
          popup.setHTML(
            `<div id="popup">Store#: ${store.id}</div><div>${store.street}, ${
              store.city
            }<br/>${store.distance.toFixed(2)} mi</div>`
          );
          marker.setPopup(popup);
        });
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
