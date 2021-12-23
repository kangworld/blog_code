# ✍️ 싱글톤 패턴

싱글톤 패턴이란 오직 한 개의 클래스 인스턴스만을 갖도록 보장하고, 이에 대한 전역적인 접근점을 제공한다.

유니티에서 싱글톤 패턴이 필요한 상황은 다음과 같다.

-   같은 Scene에서 데이터 공유
-   서로 다른 Scene에서 데이터 공유

유니티에서 싱글톤 객체 생성 핵심은 DontDestroyOnLoad 메서드이다.

1.  Scene에 Empty Object를 하나 생성한다.
2.  싱글톤 객체를 생성하는 스크립트를 작성해서 Empty Object에 연결한다.
3.  Scene이 변경돼도 Empty Object는 생존하도록 DontDestroyOnLoad를 호출한다.

# 🧐 싱글톤 패턴의 장단점

싱글톤 패턴의 객체는 일종의 전역 변수이다.

-   모든 곳에서 접근 가능하단 장점은 있지만, 싱글톤 객체의 데이터가 변경되는 시점을 정확히 알기 힘들다. 싱글톤 객체와 관련된 모든 코드를 하나하나 찾아봐야 한다.
-   전역 변수이기에 싱글톤 객체를 중심으로 커플링(coupling)현상이 발생한다. 하나의 코드를 수정하면 싱글톤 객체를 사용하는 모든 곳에서 영향을 받는다. 이는 장점이 될 수도 단점이 될 수 있다.
-   전역 변수이기에 멀티쓰레드 환경에서 임계 영역에 대한 경쟁 상태(race condition)를 피할 수 없다. 이를 해결하기 위해 lock을 사용하는데 이는 쓰레드간 병목현상을 유발해 자연스럽게 성능은 낮아진다. 또한 정확한 임계 영역을 찾는 것은 순전히 개발자의 능력이다.

그렇다면 단점이 많은 싱글톤 패턴을 사용하면 안 되는 걸까?

정답은 NO이다. 어떤 기술이든 장단점은 존재한다. 적절한 Trade-off를 계산해서 사용하자.

# 🍊 유니티 실습

유니티에선 주로 Mananger 클래스의 객체를 싱글톤 패턴으로 만든다.

실습 목표는 Sphere가 Plane과 충돌하면 Audio clip을 재생함과 동시에 Sphere를 Destroy하는 것이다.

### 실습 #1️

Sphere에 Rigidbody 컴포넌트를 추가해서 Use Gravity를 체크하고 Sphere가 Plane에 닿으면 오디오를 재생하고 Sphere를 Destroy해보자.

<b>WhenDestroyPlay.cs</b>

```cpp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhenDestroyPlay : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        gameObject.GetComponent<AudioSource>().Play();

        Destroy(gameObject);
    }
}
```

안타깝게도 Play 호출 다음 바로 Destroy를 해서 소리가 나오지 않는다.

### 실습 #2
  
Destroy에 두 번째 인자로 float 값을 넘겨주면 해당 시간만큼 Delay하고 Object를 Destroy한다.

<b>DestroyDelayed.cs</b>
```cpp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDelayed : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();    
    }
    private void OnCollisionEnter(Collision collision)
    {
        _audioSource.Play();

        Destroy(gameObject, _audioSource.clip.length);
    }
}
```

소리가 나오긴 하나 Audio cilp의 재생이 끝나고 Sphere가 Destroy된다.

### 실습 #3

앞서 등장한 문제를 해결하기 위해 싱글톤을 적용해 보자.

Sphere에 MyAudioPlay.cs를 붙이고 프리펩으로 만들자. AudioClip은 유니티상에서 드래그 앤 드롭으로 연결해 준다. 

충돌 시 AudioClip의 재생은 싱글톤 객체를 제공하는 AudioManager가 담당하고 Sphere는 Destroy된다.

<b>MyAudioPlay.cs</b>

```cpp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAudioPlay : MonoBehaviour
{
    public AudioClip AudioClip;

    private void OnCollisionEnter(Collision collision)
    {
        AudioManager.Instance().Play(AudioClip);

        Destroy(gameObject);
    }
}
```

Empty Object를 하나 만들어 이름을 GameManager로 변경하고 Audio Source 컴포넌트를 추가한다. (AudioClip은 Sphere가 Play를 호출할 때 인자로 넘겨준다.)

AudioManager.cs는 싱글톤 객체를 만드는 코드이다. Start 호출 시 static으로 선언된 \_instance에 자기 자신을 넣어주고 Instance 메서드를 통해 자기 자신을 다른 클래스에서 접근하도록 한다.

GameManager에 AudioManager.cs를 붙여준다.

<b>AudioManager.cs</b>

```cpp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance = null;

    public static AudioManager Instance()
    {
        return _instance;
    }
    
    void Start()
    {
        if (_instance == null)
            _instance = this;
    }

    public void Play(AudioClip clip)
    {
        GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
```

CreateBall.cs은 좌 클릭 시 임의의 위치로 Sphere를 생성하는 스크립트로 GameManager의 컴포넌트로 붙여준다. 

Quaternion.identity : 프리팹의 원래 회전각을 사용함

<b>CreateBall.cs</b>

```cpp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBall : MonoBehaviour
{
    public GameObject Ball;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            float x, y, z;
            x = z = Random.Range(-4, 4);
            y = Random.Range(4, 8);

            Vector3 pos = new Vector3(x, y, z);
            //Quaternion.identity : 프리팹의 원래 회전각을 사용함
            Instantiate(Ball, pos, Quaternion.identity);
        }
    }
}
```
