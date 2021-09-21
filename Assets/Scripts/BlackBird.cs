using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBird : Bird
{
    [SerializeField]
    //besar radius untuk ledakan
    private float _explosionRadius = 2f;
    //status apakah bird sudah meledak
    private bool _hasExploded = false;

    //Untuk efek meledak, agar simpel yang saya lakukan adalah membesarkan radius dari collider dari bird. Saat terjadi kolisi
    //dengan objek apapun, maka collider tersebut akan membesar, dan bird akan hilang karena meledak
    public override void OnCollisionEnter2D(Collision2D col)
    {
        if (!_hasExploded)
        {
            Debug.Log("Exploooooossionn");
            _hasExploded = true;
            base.OnCollisionEnter2D(col);
            Collider.radius = _explosionRadius;

            //bird akan dihancurkan segera setelah ledakan terjadi. Diberi selang waktu 0.1 detik untuk waktu efek ledakan
            //bereaksi sebelum bird hilang. Agar bird bisa langsung hilang, maka spritenya akan langsung dinonaktifkan.
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.enabled = false;
            _flagDestroy = true;
            StartCoroutine(DestroyAfter(0.1f));
        }

        string tag = col.gameObject.tag;
        
        //jika collider yang membesar mengenai obstacle/enemy, maka obstacle/enemy tersebut akan langsung dihancurkan
        if (tag == "Bird" || tag == "Obstacle")
        {
            Destroy(col.gameObject);
        }

        //hanya ditambahkan agar bird tidak sempat bergelinding lagi selama 0.1 detik setelah bird meledak
        if (tag == "Ground")
        {
            Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), Collider);
        }
    }

}
