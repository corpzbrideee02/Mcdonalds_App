<template>
  <div class="container">
    <img class="item-image" style="margin-top: 2vh" src="../assets/tray.png" />
    <div class="heading">Tray Contents</div>
    <div class="status">{{ state.status }}</div>
    <div v-if="state.tray.length > 0" id="tray">
      <DataTable
        :value="state.tray"
        :scrollable="true"
        scrollHeight="38vh"
        dataKey="id"
        class="p-datatable-striped"
        id="tray-contents"
      >
        <Column header="Qty" field="qty" style="margin-right: -70vw" />
        <Column id="desc" header="Description" field="item.description" />
      </DataTable>
    </div>
    <div v-if="state.tray.length > 0">
      <div class="tray-head">Tray Totals</div>
      <table style="border: solid; margin-top: 1vh">
        <tr>
          <td style="width: 20%; font-weight: bold">Protein</td>
          <td style="width: 10%; text-align: right; padding-right: 3vw">
            {{ state.proTot }}
          </td>
          <td style="width: 20%; font-weight: bold">Calories</td>
          <td style="width: 10%; text-align: right; padding-right: 3vw">
            {{ state.calTot }}
          </td>
          <td style="width: 20%; font-weight: bold">Carbs.</td>
          <td style="width: 10%; text-align: right; padding-right: 3vw">
            {{ state.carbTot }}
          </td>
        </tr>
        <tr>
          <td style="width: 20%; font-weight: bold">Fibre</td>
          <td style="width: 10%; text-align: right; padding-right: 3vw">
            {{ state.fbrTot }}
          </td>
          <td style="width: 20%; font-weight: bold">Choles.</td>
          <td style="width: 10%; text-align: right; padding-right: 3vw">
            {{ state.cholTot }}
          </td>
          <td style="width: 20%; font-weight: bold">Salt</td>
          <td style="width: 10%; text-align: right; padding-right: 3vw">
            {{ state.saltTot }}
          </td>
        </tr>
        <tr>
          <td style="width: 20%; font-weight: bold">Fat</td>
          <td style="width: 10%; text-align: right; padding-right: 3vw">
            {{ state.fatTot }}
          </td>
          <td colspan="4">&nbsp;</td>
        </tr>
      </table>
      <Button
        style="margin-top: 2vh"
        label="Save Tray"
        @click="saveTray"
        class="p-button-outlined margin-button1"
      />
      <Button
        style="margin-top: 2vh"
        label="Clear Tray"
        @click="clearTray"
        class="p-button-outlined margin-button1"
      />
    </div>
  </div>
</template>

<script>
import { reactive, onMounted } from "vue";
import { fetcher,poster } from "../util/apiutil";
import { useRouter } from "vue-router";
export default {
  setup() {
    const router = useRouter();
    onMounted(() => {
      if (sessionStorage.getItem("tray")) {
        state.tray = JSON.parse(sessionStorage.getItem("tray"));
        state.tray.map((trayItem) => {
          state.fbrTot += trayItem.item.fibre * trayItem.qty;
          state.calTot += trayItem.item.calories * trayItem.qty;
          state.saltTot += trayItem.item.salt * trayItem.qty;
          state.fatTot += trayItem.item.fat * trayItem.qty;
          state.cholTot += trayItem.item.cholesterol * trayItem.qty;
          state.proTot += trayItem.item.protein * trayItem.qty;
          state.carbTot += trayItem.item.carbs * trayItem.qty;
        });
      } else {
        state.tray = [];
      }
    });
    let state = reactive({
      status: "",
      fbrTot: 0,
      calTot: 0,
      saltTot: 0,
      fatTot: 0,
      cholTot: 0,
      proTot: 0,
      carbTot: 0,
      tray: [],
    });

    const clearTray = () => {
      sessionStorage.removeItem("tray");
      state.tray = [];
      state.status = "tray cleared";
    };
    const saveTray = async () => {
      let user = JSON.parse(sessionStorage.getItem("user"));
      let tray = JSON.parse(sessionStorage.getItem("tray"));
      try {
        state.status = "sending tray info to server";
        let trayHelper = { email: user.email, selections: tray };
        let payload = await poster("tray", trayHelper);
        if (payload.indexOf("not") > 0) {
          state.status = payload;
        } else {
          clearTray();
          state.status = payload;
        }
      } catch (err) {
        console.log(err);
        state.status = `Error add tray: ${err}`;
      }
    };

    return {
      state,
      clearTray,
      saveTray
    };
  },
};
</script>

<style>
#traycontents th:nth-child(2) {
  margin-left: 45vw;
}
</style>
