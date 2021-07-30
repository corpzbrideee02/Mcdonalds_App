<template>
  <div>
    <div class="heading">Categories</div>
    <div class="status">{{ state.status }}</div>
    <Dropdown
      v-if="state.categories.length > 0"
      v-model="state.selectedCategory"
      style="text-align: left"
      :options="state.categories"
      optionLabel="name"
      optionValue="id"
      placeholder="Select Category"
      @change="loadMenuitems"
    />
    <div style="margin-top: 2vh" v-if="state.menuitems.length > 0">
      <DataTable
        :value="state.menuitems"
        :scrollable="true"
        scrollHeight="60vh"
        selectionMode="single"
        class="p-datatable-striped"
        @row-select="onRowSelect"
      >
        <Column style="margin-right: -35vw">
          <template #body="slotProps">
            <img
              :src="`/burger.jpg`"
              :alt="slotProps.data.description"
              class="item-image"
            />
          </template>
        </Column>
        <Column field="description" header="Select an Item"></Column>
      </DataTable>

      <Dialog v-model:visible="state.itemSelected" class="dialog-border">
        <div style="text-align: center">
          <div style="margin-bottom: 2vh; font-size: larger; font-weight: bold">
            {{ state.selectedItem.description }}
          </div>
          <img
            :src="`/burger.jpg`"
            :alt="state.selectedItem.description"
            class="item-image"
          />
        </div>
        <div
          style="
            font-weight: bold;
            font-size: larger;
            margin-top: 3vh;
            text-align: center;
          "
        >
          Nutrional Info
        </div>
        <table style="border: solid; margin-top: 2vh">
          <tr>
            <td style="width: 20%; font-weight: bold">Protein</td>
            <td style="width: 10%; text-align: right; padding-right: 3vw">
              {{ state.selectedItem.protein }}
            </td>
            <td style="width: 20%; font-weight: bold">Calories</td>
            <td style="width: 10%; text-align: right; padding-right: 3vw">
              {{ state.selectedItem.calories }}
            </td>
            <td style="width: 20%; font-weight: bold">Carbs.</td>
            <td style="width: 10%; text-align: right; padding-right: 3vw">
              {{ state.selectedItem.carbs }}
            </td>
          </tr>
          <tr>
            <td style="width: 20%; font-weight: bold">Fibre</td>
            <td style="width: 10%; text-align: right; padding-right: 3vw">
              {{ state.selectedItem.fibre }}
            </td>
            <td style="width: 20%; font-weight: bold">Choles.</td>
            <td style="width: 10%; text-align: right; padding-right: 3vw">
              {{ state.selectedItem.cholesterol }}
            </td>
            <td style="width: 20%; font-weight: bold">Salt</td>
            <td style="width: 10%; text-align: right; padding-right: 3vw">
              {{ state.selectedItem.salt }}
            </td>
          </tr>
          <tr>
            <td style="width: 20%; font-weight: bold">Fat</td>
            <td style="width: 10%; text-align: right; padding-right: 3vw">
              {{ state.selectedItem.fat }}
            </td>
            <td colspan="4">&nbsp;</td>
          </tr>
        </table>

        <!-- Quantity input -->
        <div style="margin-top: 2vh; text-align: center">
          <span style="margin-left: -10vw; margin-right: 2vw">Qty:</span>
          <span>
            <InputNumber
              id="qty"
              :min="0"
              v-model="state.qty"
              :step="1"
              incrementButtonClass="plus"
              showButtons
              buttonLayout="horizontal"
              decrementButtonIcon="pi pi-minus"
              incrementButtonIcon="pi pi-plus"
            />
          </span>
        </div>

        <!-- Add to Tray and View Tray buttons -->
        <div style="text-align: center; margin-top: 2vh">
          <Button
            label="Add To Tray"
            @click="addToTray"
            class="p-button-outlined margin-button1"
          />
           &nbsp;
        <Button
          label="View Tray"
          class="p-button-outlined margin-button1"
          v-if="state.tray.length > 0"
          @click="viewTray"
        />
        </div>
       
        <!-- Dialog Status -->
        <div
          style="text-align: center"
          v-if="state.dialogStatus !== ''"
          class="dialog-status"
        >
          {{ state.dialogStatus }}
        </div>
      </Dialog>
    </div>
  </div>
</template>

<script>
import { reactive, onMounted } from "vue";
import { fetcher } from "../util/apiutil";
import { useRouter } from "vue-router";
export default {
  setup() {
  const router = useRouter();
    onMounted(() => {
      loadCategories();
    });
    let state = reactive({
      status: "",
      categories: [],
      menuitems: [],
      selectedCategory: {},
      selectedItem: {},
      dialogStatus: "",
      itemSelected: false,
      qty: 0,
      tray: [],
    });
    // const loadCategories = async () => {
    //   let url = "https://localhost:44371/api/Category";
    //   try {
    //     state.status = "finding categories ...";
    //     let response = await fetch(`${url}`);
    //     state.categories = await response.json();
    //     state.status = `loaded ${state.categories.length} categories`;
    //   } catch (err) {
    //     console.log(err);
    //     state.status = `Error has occured: ${err.message}`;
    //   }
    // };

    const loadMenuitems = async () => {
      try {
        state.status = `finding menuitems for category ${state.selectedCategory}...`;
        let payload = await fetcher(`Menuitem/${state.selectedCategory}`);
        state.menuitems = payload;
        state.status = `loaded ${state.menuitems.length} menu items`;
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
      if (sessionStorage.getItem("tray")) {
        state.tray = JSON.parse(sessionStorage.getItem("tray"));
      }
    };
    const loadCategories = async () => {
      try {
        state.status = "loading categories...";
        const payload = await fetcher(`Category`);
        if (payload.error === undefined) {
          state.categories = payload;
          state.status = `loaded ${state.categories.length} categoriess`;
        } else {
          state.status = payload.error;
        }
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };

    const onRowSelect = (event) => {
      try {
        state.selectedItem = event.data;
        state.dialogStatus = "";
        state.itemSelected = true;
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };

    const addToTray = () => {
      const index = state.tray.findIndex(
        // is item already on the tray
        (item) => item.id === state.selectedItem.id
      );
      if (state.qty !== 0) {
        index === -1 // add
          ? state.tray.push({
              id: state.selectedItem.id,
              qty: state.qty,
              item: state.selectedItem,
            })
          : (state.tray[index] = {
              // replace
              id: state.selectedItem.id,
              qty: state.qty,
              item: state.selectedItem,
            });
        state.dialogStatus = `${state.qty} item(s) added`;
      } else {
        index === -1 ? null : state.tray.splice(index, 1); // remove
        state.dialogStatus = `item(s) removed`;
      }
      sessionStorage.setItem("tray", JSON.stringify(state.tray));
      state.qty = 0;
    };

    const viewTray = () => {
router.push("tray");
};

    return {
      state,
      loadMenuitems,
      onRowSelect,
      addToTray,
      viewTray
    };
  },
};
</script>
