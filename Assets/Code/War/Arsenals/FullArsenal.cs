namespace War.Arsenals {

    public class FullArsenal : Arsenal {

        public FullArsenal () {
            for (int i = 0; i < 256; i++) AddAmmo (i, -1);
        }

    }

}
